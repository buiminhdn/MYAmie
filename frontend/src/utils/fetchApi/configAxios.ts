import axios, {
  AxiosError,
  AxiosInstance,
  AxiosRequestConfig,
  AxiosResponse,
} from "axios";

interface Configure {
  configure: AxiosRequestConfig;
  getAccessToken: () => string;
  getRefreshToken: () => string;
}

type Success<ResponseDataT> = (
  res: AxiosResponse<ResponseDataT>,
  originalRequest: AxiosRequestConfig
) => void;

type Failure = (error: AxiosError) => void;

interface AccessTokenParams {
  setCondition: (config: AxiosRequestConfig) => boolean;
}

interface RefreshTokenConfig<ResponseDataT, AxiosDataReturnT> {
  setCondition: (error: AxiosError) => boolean;
  axiosData: (accessToken: string, refreshToken: string) => AxiosDataReturnT;
  success: Success<ResponseDataT>;
  failure: Failure;
}

const { CancelToken } = axios;

export default class ConfigureAxios {
  private axiosInstance: AxiosInstance;
  private getAccessToken: () => string;
  private getRefreshToken: () => string;

  public constructor({
    configure,
    getAccessToken,
    getRefreshToken,
  }: Configure) {
    this.getAccessToken = getAccessToken;
    this.getRefreshToken = getRefreshToken;
    this.axiosInstance = axios.create(configure);
  }

  public create = (cancel = "") => {
    return {
      request: (requestConfig: AxiosRequestConfig) => {
        const source = CancelToken.source();

        const request = this.axiosInstance({
          ...requestConfig,
          cancelToken: source.token,
        });
        if (!!cancel) {
          // @ts-ignore
          request[cancel] = source.cancel;
        }
        return request;
      },
    };
  };

  public accessToken = ({ setCondition }: AccessTokenParams) => {
    this.axiosInstance.interceptors.request.use((config) => {
      if (!config.url) return config;

      const accessToken = this.getAccessToken();

      if (
        setCondition(config) &&
        !config.headers.Authorization &&
        accessToken
      ) {
        config.headers.Authorization = `Bearer ${accessToken}`;
      }

      return config;
    });
  };

  public _handleRefreshToken = async <
    ResponseDataT extends any,
    AxiosDataReturnT extends any
  >(
    config: RefreshTokenConfig<ResponseDataT, AxiosDataReturnT>,
    error: AxiosError
  ) => {
    const { axiosData, success, failure } = config;

    try {
      const accessToken = this.getAccessToken();
      const refreshToken = this.getRefreshToken();

      const res = await this.axiosInstance.post(
        "/Token/refresh-token",
        axiosData(accessToken, refreshToken)
      );

      if (error.config) {
        success(res, error.config);

        return await this.axiosInstance.request(error.config);
      } else {
        throw new Error("Original request config is undefined.");
      }
    } catch (err) {
      failure(error);
      return await Promise.reject(error);
    } finally {
      this.refreshToken(config);
    }
  };

  public refreshToken = <
    ResponseDataT extends any = any,
    AxiosDataReturnT = any
  >(
    config: RefreshTokenConfig<ResponseDataT, AxiosDataReturnT>
  ) => {
    const interceptor = this.axiosInstance.interceptors.response.use(
      undefined,
      (error: AxiosError) => {
        if (!config.setCondition(error)) {
          return Promise.reject(error);
        }

        this.axiosInstance.interceptors.response.eject(interceptor);

        return this._handleRefreshToken(config, error);
      }
    );
  };
}

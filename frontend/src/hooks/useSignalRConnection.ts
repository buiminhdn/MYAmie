import { useEffect, useState } from "react";
import * as signalR from "@microsoft/signalr";
import { useSelector } from "react-redux";
import { tokenSelector } from "@/store/auth/auth.selector";
import env from "@/env";
import toast from "react-hot-toast";

const useSignalRConnection = (currentUserId?: number, otherUserId?: number) => {
  const [connection, setConnection] = useState<signalR.HubConnection | null>(
    null
  );
  const accessToken = useSelector(tokenSelector);

  useEffect(() => {
    if (!accessToken || !currentUserId || !otherUserId) return;

    const newConnection = new signalR.HubConnectionBuilder()
      .withUrl(env.apiChatEndPoint, {
        accessTokenFactory: () => accessToken,
      })
      .withAutomaticReconnect()
      .configureLogging(signalR.LogLevel.None)
      .build();

    setConnection(newConnection);

    return () => {
      if (connection) connection.stop();
    };
  }, [accessToken, currentUserId, otherUserId]);

  useEffect(() => {
    if (!connection) return;

    connection
      .start()
      .then()
      .catch(() => {
        toast.error("Xảy ra lỗi khi kết nối tới máy chủ, thử lại sau");
      });

    return () => {
      if (connection) connection.stop();
    };
  }, [connection]);

  return connection;
};

export default useSignalRConnection;

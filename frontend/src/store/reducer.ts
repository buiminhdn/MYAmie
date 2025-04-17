import { combineReducers } from "@reduxjs/toolkit";
import authReducer from "./auth/auth.slice";

const reducers = {
  auth: authReducer,
};

export default combineReducers(reducers);

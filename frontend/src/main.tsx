import { createRoot } from "react-dom/client";
import App from "./App.tsx";
import "./index.css";
import "./assets/styles/global.css";
import "./assets/icons/all.css";
import Modal from "react-modal";

Modal.setAppElement("#root");

createRoot(document.getElementById("root")!).render(
  // <StrictMode>
  <App />
  // </StrictMode>
);

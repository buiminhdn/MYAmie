// tests/utils.ts
import { Page } from "@playwright/test";
import { BASE_URL, CREDENTIALS } from "./config";

/**
 * Logs in a user with the provided role.
 */
export async function login(page: Page, role: "USER" | "ADMIN") {
  await page.goto(`${BASE_URL}/login`);
  await page.fill("#login-email", CREDENTIALS[role].email);
  await page.fill("#login-password", CREDENTIALS[role].password);
  await page.waitForFunction(() => {
    const btn = document.querySelector("#login-button");
    return btn && !btn.hasAttribute("disabled");
  });
  await page.click("#login-button");
  await page.waitForURL(BASE_URL);
}

/**
 * Logs out the current user.
 */
export async function logout(page: Page) {
  await page.click("#profile-options img");
  await page.click("#logout-button");
  await page.waitForURL(BASE_URL);
}

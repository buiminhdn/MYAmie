import { test, expect } from "@playwright/test";
import { ROUTE_PATH } from "../src/routes/route-path";
import { BASE_URL } from "./config";
import { login } from "./utils";

test.describe("Role-Based Access Control (RBAC)", () => {
  // Test 1: Ensure unauthenticated users are redirected from private pages
  test("Unauthenticated users should be redirected from private pages", async ({
    page,
  }) => {
    await page.goto(`${BASE_URL}${ROUTE_PATH.SETTINGS}`);
    await page.waitForURL(`${BASE_URL}${ROUTE_PATH.LOGIN}`);
    await expect(page.locator("#login-button")).toBeVisible();
  });

  // Test 2: Regular users should NOT access Admin-only routes
  test("Regular users should not access Admin pages", async ({ page }) => {
    // Login as regular user
    await login(page, "USER");

    // Try accessing admin-only route
    await page.goto(`${BASE_URL}${ROUTE_PATH.ADMIN_USERS}`);
    await page.goto(`${BASE_URL}${ROUTE_PATH.LOGIN}`);
  });

  // Test 3: Admin users should access Admin pages successfully
  test("Admin users should be able to access Admin pages", async ({ page }) => {
    // Login as admin user
    await login(page, "ADMIN");

    // Access admin-only route
    await page.goto(`${BASE_URL}${ROUTE_PATH.ADMIN_USERS}`);
    await expect(
      page.getByText("Quản lý người dùng", { exact: true })
    ).toBeVisible();
  });
});

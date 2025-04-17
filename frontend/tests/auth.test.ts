import { ROUTE_PATH } from "../src/routes/route-path";
import { test, expect } from "@playwright/test";
import { BASE_URL } from "./config";
import { login, logout } from "./utils";

test.describe("Authentication - Login", () => {
  // Test 1: Verify that the login page loads correctly
  test("Login page should load properly", async ({ page }) => {
    await page.goto(`${BASE_URL}${ROUTE_PATH.LOGIN}`);
    await expect(page.locator("#login-title")).toHaveText("Đăng nhập");
    await expect(page.locator("#login-email")).toBeVisible();
    await expect(page.locator("#login-password")).toBeVisible();
    await expect(page.locator("#login-button")).toBeVisible();
  });

  // Test 2: Form validation - empty fields
  test("Should show validation messages for empty fields", async ({ page }) => {
    await page.goto(`${BASE_URL}${ROUTE_PATH.LOGIN}`);
    await page.click("#login-button");
    await expect(page.locator("#login-email-error")).toHaveText(
      "Email không được để trống"
    );
    await expect(page.locator("#login-password-error")).toHaveText(
      "Mật khẩu không được để trống"
    );
  });

  // Test 3: Form validation - invalid email format
  test("Should show error for invalid email format", async ({ page }) => {
    await page.goto(`${BASE_URL}${ROUTE_PATH.LOGIN}`);
    await page.fill("#login-email", "invalid-email");
    await page.fill("#login-password", "validpassword");
    await page.click("#login-button");
    await expect(page.locator("#login-email-error")).toHaveText(
      "Email không đúng định dạng"
    );
  });

  // Test 4: Form validation - short password
  test("Should show error for short password", async ({ page }) => {
    await page.goto(`${BASE_URL}${ROUTE_PATH.LOGIN}`);
    await page.fill("#login-email", "test@example.com");
    await page.fill("#login-password", "123");
    await page.click("#login-button");
    await expect(page.locator("#login-password-error")).toHaveText(
      "Mật khẩu phải dài hơn 6 ký tự"
    );
  });

  // Test 5: Successful login
  test("Should log in successfully with valid credentials", async ({
    page,
  }) => {
    await login(page, "USER");
    await page.waitForURL(BASE_URL);
    await expect(page.locator("#profile-options")).toBeVisible();
  });

  // Test 7: Logout
  test("Should log out successfully", async ({ page }) => {
    await login(page, "USER");
    await page.click("#profile-options img");
    await page.click("#logout-button");
    await page.waitForURL(BASE_URL);
    await expect(page.locator("#access-button")).toBeVisible();
  });
});

test.describe("Authentication - Sign Up", () => {
  // Test 1: Verify that the signup page loads correctly
  test("Signup page should load properly", async ({ page }) => {
    await page.goto(`${BASE_URL}${ROUTE_PATH.SIGNUP}`);
    await expect(page.locator("#signup-lastname")).toBeVisible();
    await expect(page.locator("#signup-firstname")).toBeVisible();
    await expect(page.locator("#signup-email")).toBeVisible();
    await expect(page.locator("#signup-password")).toBeVisible();
    await expect(page.locator("#signup-button")).toBeVisible();
  });

  // Test 2: Form validation - empty fields
  test("Should show validation messages for empty fields", async ({ page }) => {
    await page.goto(`${BASE_URL}${ROUTE_PATH.SIGNUP}`);
    await page.click("#signup-button");
    await expect(page.locator("#signup-email-error")).toHaveText(
      "Email không được để trống"
    );
    await expect(page.locator("#signup-password-error")).toHaveText(
      "Mật khẩu phải dài hơn 6 ký tự"
    );
    await expect(page.locator("#signup-lastname-error")).toHaveText(
      "Họ không được để trống"
    );
    await expect(page.locator("#signup-firstname-error")).toHaveText(
      "Tên không được để trống"
    );
  });

  // Test 3: Form validation - invalid email format
  test("Should show error for invalid email format", async ({ page }) => {
    await page.goto(`${BASE_URL}${ROUTE_PATH.SIGNUP}`);
    await page.fill("#signup-email", "invalid-email");
    await page.click("#signup-button");
    await expect(page.locator("#signup-email-error")).toHaveText(
      "Email không đúng định dạng"
    );
  });

  // Test 4: Form validation - short password
  test("Should show error for short password", async ({ page }) => {
    await page.goto(`${BASE_URL}${ROUTE_PATH.SIGNUP}`);
    await page.fill("#signup-password", "123");
    await page.click("#signup-button");
    await expect(page.locator("#signup-password-error")).toHaveText(
      "Mật khẩu phải dài hơn 6 ký tự"
    );
  });

  // Test 5: Form validation - first and last name length
  test("Should show error for long first/last name", async ({ page }) => {
    await page.goto(`${BASE_URL}${ROUTE_PATH.SIGNUP}`);
    await page.fill("#signup-lastname", "a".repeat(51));
    await page.fill("#signup-firstname", "a".repeat(51));
    await page.click("#signup-button");
    await expect(page.locator("#signup-lastname-error")).toHaveText(
      "Họ không được dài quá 50 ký tự"
    );
    await expect(page.locator("#signup-firstname-error")).toHaveText(
      "Tên không được dài quá 50 ký tự"
    );
  });
});

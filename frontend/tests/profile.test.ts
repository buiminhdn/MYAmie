import { test, expect } from "@playwright/test";
import { login } from "./utils";
import { BASE_URL } from "./config";

test.describe("Profile Update", () => {
  test.beforeEach(async ({ page }) => {
    await login(page, "USER");
    await page.goto(`${BASE_URL}/info`);
  });

  test("Ensure the settings page loads correctly", async ({ page }) => {
    await expect(page.locator("text=Ảnh bìa (1400 x 320px)")).toBeVisible();
    await expect(page.locator("text=Ảnh đại diện")).toBeVisible();
    await expect(page.locator("text=Họ").first()).toBeVisible();
    await expect(page.locator("text=Tên")).toBeVisible();
  });

  test("Ensure input fields update correctly", async ({ page }) => {
    await page.fill("input[placeholder='Nhập họ']", "Nguyễn Thị"); // The input value is now "Nguyễn Thị"
    await page.fill("input[placeholder='Nhập tên']", "Văn A");
    await page.fill("input[type='date']", "1995-08-15");

    // Update expected value to match the filled value
    await expect(page.locator("input[placeholder='Nhập họ']")).toHaveValue(
      "Nguyễn Thị"
    );
    await expect(page.locator("input[placeholder='Nhập tên']")).toHaveValue(
      "Văn A"
    );
    await expect(page.locator("input[type='date']")).toHaveValue("1995-08-15");
  });
});

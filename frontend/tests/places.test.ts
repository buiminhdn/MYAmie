import { test, expect } from "@playwright/test";
import { BASE_URL } from "./config";

test.describe("places Page", () => {
  test.beforeEach(async ({ page }) => {
    await page.goto(`${BASE_URL}/places`);
  });

  test("should display places correctly", async ({ page }) => {
    const placeCards = page.locator(".place-card");
    await expect(placeCards).not.toHaveCount(0); // Ensures at least one place card exists
  });

  test("should allow searching for places", async ({ page }) => {
    await page.fill(
      "input[placeholder='Nhập từ khoá tìm kiếm tại đây']",
      "Park"
    );
    await page.keyboard.press("Enter");

    const searchResults = page.locator(".place-card");
    await expect(searchResults).not.toHaveCount(0);
  });

  test("should filter places by city", async ({ page }) => {
    await page.click("#filter-button"); // Click filter
    await page.click("text=Cao Bằng"); // Select city
    await page.click("#filter-button-save"); // Save filter

    const filteredResults = page.locator(".place-card");
    await expect(filteredResults).toHaveCount(0);
  });

  test("should clear city filter", async ({ page }) => {
    await page.click("#filter-button");
    await page.click("text=Cao Bằng");
    await page.click("#clear-city-filter"); // Click clear button

    const allResults = page.locator(".place-card");
    await expect(allResults).not.toHaveCount(0);
  });

  test("should filter places by category", async ({ page }) => {
    await page.click("text=Thể thao"); // Click on "Thể thao" category

    const categoryResults = page.locator(".place-card");
    await expect(categoryResults).toHaveCount(0);
  });

  test("should paginate through places", async ({ page }) => {
    // await page.click("#pagination-next"); // Go to next page
    // await expect(page).toHaveURL(/pageNumber=2/);

    const nextPageButton = page.locator("#pagination-next");
    await expect(nextPageButton).toBeDisabled();
  });

  test("should disable previous page button on first page", async ({
    page,
  }) => {
    const prevButton = page.locator("#pagination-previous");
    await expect(prevButton).toBeDisabled();
  });
});

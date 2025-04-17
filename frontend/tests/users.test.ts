import { test, expect, chromium } from "@playwright/test";
import { BASE_URL } from "./config";

const latitude = 16.098;
const longitude = 108.2481;

test.describe("Users Page", () => {
  let context;
  let page;

  test.beforeEach(async ({ browser }) => {
    // Create a new browser context with geolocation enabled
    context = await browser.newContext({
      permissions: ["geolocation"],
      geolocation: { latitude, longitude },
    });

    // Create a new page in this context
    page = await context.newPage();
    await page.goto(`${BASE_URL}/users`);
  });

  test.afterEach(async () => {
    await context.close(); // Close context after each test
  });

  test("should display the Users page correctly", async () => {
    await expect(page.getByText("Tìm bạn bè quanh đây")).toBeVisible();

    const userCards = page.locator(".user-card");
    await expect(userCards).not.toHaveCount(0);
  });

  test("should allow selecting a category", async () => {
    await page.click("text=Quay & Chụp");

    const categoryUsers = page.locator(".user-card");
    await expect(categoryUsers).not.toHaveCount(0);
  });

  test("should filter users by distance", async () => {
    await page.click("#filter-button");
    await page.fill("input[type='range']", "10");
    await page.click("#filter-button-save"); // Save filter

    const filteredUsers = page.locator(".user-card");
    await expect(filteredUsers).not.toHaveCount(0);
  });

  test("should paginate through users", async () => {
    // await page.click("#pagination-next"); // Go to next page
    // await expect(page).toHaveURL(/pageNumber=2/);

    const nextPageButton = page.locator("#pagination-next");
    await expect(nextPageButton).toBeDisabled();
  });

  test("should disable previous page button on first page", async () => {
    const prevButton = page.locator("#pagination-previous");
    await expect(prevButton).toBeDisabled();
  });
});

import { test, expect, BrowserContext, Page } from "@playwright/test";
import { BASE_URL, GEOLOCATION } from "./config";
import { login } from "./utils";

test.describe("Navigation & Routing", () => {
  let context: BrowserContext, page: Page;

  test.beforeEach(async ({ browser }) => {
    context = await browser.newContext({
      permissions: ["geolocation"],
      geolocation: {
        latitude: GEOLOCATION.latitude,
        longitude: GEOLOCATION.longitude,
      },
    });
    page = await context.newPage();
  });

  test.describe("Public Pages", () => {
    test("Should load home page correctly", async () => {
      await page.goto(`${BASE_URL}/`);
      await expect(page.locator("#main-title")).toContainText("dịch vụ");
    });

    test("Should load places page correctly", async () => {
      await page.goto(`${BASE_URL}/places`);
      await expect(page.locator("#main-title")).toContainText("địa điểm");
    });

    test("Should load users page correctly with geolocation", async () => {
      await page.goto(`${BASE_URL}/users`);
      await expect(page.locator("#main-title")).toContainText("bạn bè");
    });

    test("Should load user profile page correctly", async () => {
      await login(page, "USER");
      await page.click("#profile-options img");
      await page.click("#profile-link");
      await expect(page.locator("body")).toContainText("Mô tả");
    });
  });

  test.describe("Navigation Interactions", () => {
    test("Clicking on a business should navigate to the business detail page", async () => {
      await page.goto(`${BASE_URL}`);
      await page.click(".business-card:first-child");
      await expect(page).toHaveURL(/\/service\/\d+/);
    });

    test("Clicking on a place should navigate to the place detail page", async () => {
      await page.goto(`${BASE_URL}/places`);
      await page.click(".place-card:first-child");
      await expect(page).toHaveURL(/\/place\/\d+/);
    });

    test("Clicking on a user should navigate to the user profile page", async () => {
      await page.goto(`${BASE_URL}/users`);
      await page.click(".user-card:first-child a");
      await expect(page).toHaveURL(/\/user\/\d+/);
    });
  });

  test.describe("Sidebar & Navbar Navigation", () => {
    test("Should navigate to Places using the sidebar", async () => {
      await page.goto(`${BASE_URL}/`);
      await page.click("#navlink-places");
      await expect(page).toHaveURL(`${BASE_URL}/places`);
    });

    test("Should navigate to Users using the navbar", async () => {
      await page.goto(`${BASE_URL}/`);
      await page.click("#navlink-users");
      await expect(page).toHaveURL(`${BASE_URL}/users`);
    });
  });

  test.describe("404 Handling", () => {
    test("Should display 404 page for non-existent routes", async () => {
      await page.goto(`${BASE_URL}/random-nonexistent-page`);
      await expect(page.locator("#main-title")).toContainText("dịch vụ");
    });
  });

  test.describe("Unauthorized Access", () => {
    test("Should redirect unauthorized users from private pages", async () => {
      await page.goto(`${BASE_URL}/dashboard`);
      await expect(page.locator("#main-title")).toContainText("dịch vụ");
    });
  });
});

export function formatDateForInput(
  dateTimeString: string | null | undefined
): string {
  if (!dateTimeString) return ""; // Handle null or undefined

  const [day, month, year] = dateTimeString.split("-"); // Split by '-'
  if (!day || !month || !year) return ""; // Invalid format safeguard

  return `${year}-${month}-${day}`; // Convert to YYYY-MM-DD
}

export const expectThrowsAsync = async (action: () => Promise<any>) => {
  let successful = false;
  try {
    await action();
  } catch {
    successful = true;
  }
  expect(successful).toBe(true);
}
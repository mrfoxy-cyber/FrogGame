const FrogApi = (() => {
  const baseUrl = String(globalThis.FROG_API_BASE || "").replace(/\/+$/, "");

  function url(path) {
    const normalizedPath = path.startsWith("/") ? path : `/${path}`;
    return `${baseUrl}${normalizedPath}`;
  }

  return { url };
})();

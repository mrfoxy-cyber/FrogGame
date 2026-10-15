const RetroDigits = (() => {
  const assetRoot = "assets/numbers";

  function render(element, value) {
    const formatted = Number(value || 0).toLocaleString();
    const images = [];

    for (const character of formatted) {
      if (/\d/.test(character)) {
        const image = document.createElement("img");
        image.src = `${assetRoot}/${character}.png`;
        image.alt = "";
        image.className = "retro-digit";
        images.push(image);
      } else {
        const separator = document.createElement("span");
        separator.className = "retro-number-separator";
        separator.textContent = character;
        images.push(separator);
      }
    }

    element.replaceChildren(...images);
    element.setAttribute("aria-label", formatted);
  }

  return { render };
})();

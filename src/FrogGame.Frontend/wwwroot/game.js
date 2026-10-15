let ROWS = 8;
let COLS = 7;
const FROGS = [
  { name: "Blue", src: "assets/bluefrog.png", tileFilter: "brightness(0) saturate(100%) invert(57%) sepia(76%) saturate(608%) hue-rotate(157deg) brightness(93%) contrast(94%)" },
  { name: "Green", src: "assets/greenfrog.png", tileFilter: "brightness(0) saturate(100%) invert(77%) sepia(38%) saturate(850%) hue-rotate(47deg) brightness(97%) contrast(88%)" },
  { name: "Orange", src: "assets/orangefrog.png", tileFilter: "brightness(0) saturate(100%) invert(67%) sepia(98%) saturate(855%) hue-rotate(342deg) brightness(103%) contrast(101%)" },
  { name: "Pink", src: "assets/pinkfrog.png", tileFilter: "brightness(0) saturate(100%) invert(70%) sepia(47%) saturate(989%) hue-rotate(289deg) brightness(102%) contrast(100%)" },
  { name: "Lemon", src: "assets/yellowfrog.png", tileFilter: "brightness(0) saturate(100%) invert(91%) sepia(62%) saturate(816%) hue-rotate(332deg) brightness(107%) contrast(100%)" },
  { name: "Red", src: "assets/redfrog.png", tileFilter: "brightness(0) saturate(100%) invert(31%) sepia(88%) saturate(1755%) hue-rotate(334deg) brightness(103%) contrast(91%)" },
];
const SOUNDS = {
  start: ["sound-frog"],
  connect: ["sound-connect-1", "sound-connect-2", "sound-connect-3", "sound-connect-4", "sound-connect-5"],
  drop: ["sound-fart", "sound-fart-2", "sound-fart-3", "sound-fart-4", "sound-fart-5"],
  clear: ["sound-combo-1", "sound-combo-2", "sound-combo-3", "sound-fantastic-1", "sound-fantastic-2", "sound-hurray-1", "sound-hurray-2", "sound-hurray-3"],
  error: ["sound-fart", "sound-fart-2", "sound-fart-3", "sound-fart-4", "sound-fart-5"],
  win: ["sound-wooo"],
};

FROGS.forEach(frog => {
  const data = GAME_ASSETS.frogs[frog.name === "Lemon" ? "Yellow" : frog.name];
  frog.points = data.points;
  frog.color = data.color;
});
const animationStates = new Map();
let previousConnections = new Set();
const spriteImages = new Map();
const stateNames = { R: "Right", L: "Left", U: "Up", D: "Down" };
function spriteImage(src) {
  if (!spriteImages.has(src)) {
    const image = new Image();
    image.src = src;
    spriteImages.set(src, image);
  }
  return spriteImages.get(src);
}
Object.values(GAME_ASSETS.clips).forEach(clip => clip.frames.forEach(frame => spriteImage(frame.src)));
Object.values(GAME_ASSETS.tiles).forEach(tile => spriteImage(tile.src));
GAME_ASSETS.cloudEat.frames.forEach(frame => spriteImage(frame.src));

function paintSprite(canvas, sprite, frog) {
  const image = spriteImage(sprite.src);
  if (!image.complete || !image.naturalWidth) return;
  const context = canvas.getContext("2d");
  const width = image.naturalWidth * 2 / (sprite.pixelsPerUnit / 100);
  const height = image.naturalHeight * 2 / (sprite.pixelsPerUnit / 100);
  const x = (canvas.width - width) / 2, y = (canvas.height - height) / 2;
  context.clearRect(0, 0, canvas.width, canvas.height);
  context.globalCompositeOperation = "source-over";
  context.drawImage(image, x, y, width, height);
  context.globalCompositeOperation = "multiply";
  context.fillStyle = `rgb(${frog.color.join(",")})`;
  context.fillRect(0, 0, canvas.width, canvas.height);
  context.globalCompositeOperation = "destination-in";
  context.drawImage(image, x, y, width, height);
  context.globalCompositeOperation = "source-over";
}

const boardElement = document.querySelector("#board");
const nextFrogElement = document.querySelector("#next-frog");
const messageElement = document.querySelector("#message");
const scoreElement = document.querySelector("#score");
const soundToggle = document.querySelector("#sound-toggle");
const backgroundMusic = document.querySelector("#background-music");
const helpDialog = document.querySelector("#help-dialog");
const intro = document.querySelector("#intro");
const introVideo = document.querySelector("#intro-video");
const introVideoPanel = document.querySelector("#intro-video-panel");
const watchVideoButton = document.querySelector("#play-video-sound");
const gameShell = document.querySelector(".game-shell");
const scoresDialog = document.querySelector("#scores-dialog");
const scoreBreakdown = document.querySelector("#score-breakdown");
const boardStage = document.querySelector("#board-stage");
const evilCloud = document.querySelector("#evil-cloud");
const cloudCarrier = document.querySelector("#cloud-carrier");
const fallingFrog = document.querySelector("#falling-frog");
const comboElement = document.querySelector("#combo");
let cloudAnimation = null;
const idleCloudSource = "assets/evil-cloud.png";

function paintCloud(now) {
  if (!cloudAnimation) return;
  const elapsed = (now - cloudAnimation.started) / 1000;
  const frame = FrogLogic.frameAt(GAME_ASSETS.cloudEat, elapsed % GAME_ASSETS.cloudEat.duration);
  if (spriteImage(frame.src).complete && evilCloud.getAttribute("src") !== frame.src) evilCloud.src = frame.src;
}

function gridSpace() {
  const viewportHeight = window.visualViewport?.height || window.innerHeight;
  const header = document.querySelector(".game-header");
  const controls = document.querySelector(".actions");
  const lane = document.querySelector(".cloud-lane");
  const availableHeight = Math.max(40, viewportHeight - header.offsetHeight - controls.offsetHeight
    - messageElement.offsetHeight - scoreBreakdown.offsetHeight - lane.offsetHeight - 60);
  const availableWidth = Math.max(40, document.querySelector(".game-card").clientWidth - 12);
  return { width: availableWidth, height: availableHeight };
}
function fitBoard() {
  if (typeof window === "undefined" || gameShell.hidden) return;
  const space = gridSpace();
  const layout = GridLayout.fit(space.width, space.height, COLS, ROWS);
  boardStage.style.setProperty("--board-width", `${layout.width}px`);
  boardStage.style.setProperty("--grid-cols", COLS);
  boardStage.style.setProperty("--grid-rows", ROWS);
}
if (typeof window !== "undefined") {
  window.addEventListener("resize", fitBoard);
  window.visualViewport?.addEventListener("resize", fitBoard);
  if (typeof ResizeObserver !== "undefined") {
    const observer = new ResizeObserver(() => requestAnimationFrame(fitBoard));
    [document.querySelector(".game-header"), scoreBreakdown, messageElement].forEach(element => observer.observe(element));
  }
}

let board = [];
let score = 0;
let level = 0;
let nextFrog = null;
let locked = false;
let soundEnabled = true;
let soundPlaybackFailed = false;
let gameGeneration = 0;
let gameId = null;
let runSaved = false;

function frogFromColor(color) {
  return FROGS.find(frog => frog.name === color) ?? null;
}

function boardFromApi(apiBoard) {
  return apiBoard.map(row => row.map(color => color ? frogFromColor(color) : null));
}

async function gameApi(path, options = {}) {
  const response = await fetch(FrogApi.url(path), {
    headers: { "Content-Type": "application/json" },
    ...options,
  });
  const body = await response.json().catch(() => ({}));
  if (!response.ok) throw new Error(body.error || `Game API returned ${response.status}`);
  return body;
}

function reportSoundError(error) {
    soundToggle.textContent = "♪!";
    soundToggle.title = `Sound could not play: ${error.message}`;
    soundToggle.setAttribute("aria-label", "Sound failed — click to retry");
    setMessage("Sound couldn’t start. Click the sound button to try again.");
    soundPlaybackFailed = true;
}

function playSound(name) {
  if (!soundEnabled) return;
  const choices = SOUNDS[name];
  frogAudio.play(choices[Math.floor(Math.random() * choices.length)], name === "start").catch(reportSoundError);
}

function playBackgroundMusic() {
  if (!soundEnabled || gameShell.hidden) return;
  backgroundMusic.volume = .28;
  backgroundMusic.muted = false;
  backgroundMusic.play().catch(reportSoundError);
}

function updateHud() {
  RetroDigits.render(scoreElement, score);
  ScoreStore.update(score);
}

function saveRun() {
  if (runSaved || score <= 0) return;
  ScoreStore.finish(score);
  runSaved = true;
}

async function showTopScores() {
  await ScoreStore.refresh();
  RetroDigits.render(document.querySelector("#top-score-value"), ScoreStore.best());
  RetroDigits.render(document.querySelector("#top-combo-value"), ScoreStore.bestCombo());
  scoresDialog.showModal();
}

function setMessage(message) {
  messageElement.textContent = message;
}

function renderNextFrog() {
  cloudCarrier.classList.remove("is-dropping");
  nextFrogElement.replaceChildren();
  if (!nextFrog) return;
  const label = document.createElement("span");
  label.textContent = "Next";
  label.className = "next-label";
  const image = document.createElement("img");
  image.src = nextFrog.src;
  image.alt = `${nextFrog.name} frog`;
  nextFrogElement.append(image, label);
}

function renderBoard(animated = null, popping = []) {
  boardElement.replaceChildren();
  const popIds = new Set(popping.map(({ row, col }) => `${row}-${col}`));
  const activeKeys = new Set();
  for (let row = 0; row < ROWS; row++) for (let col = 0; col < COLS; col++) {
    const cell = document.createElement("button");
    const frog = board[row][col];
    const key = `${row}-${col}`;
    cell.type = "button";
    cell.className = "cell";
    cell.dataset.row = row;
    cell.dataset.col = col;
    cell.disabled = locked;
    cell.setAttribute("role", "gridcell");
    cell.setAttribute("aria-label", `${frog ? frog.name + " frog. " : ""}Drop a frog in column ${col + 1}`);
    cell.addEventListener("pointerdown", event => {
      if (event.pointerType === "touch" && !locked) cell.classList.add("touch-target");
    });
    if (animated && animated.row === row && animated.col === col) cell.classList.add("drop");
    if (frog) {
      activeKeys.add(key);
      const placement = FrogLogic.placement(board, row, col);
      const exploding = popIds.has(key);
      const signature = frog.name + ":" + (exploding ? "explodingfrogs" : placement);
      let state = animationStates.get(key);
      if (!state || state.signature !== signature) {
        state = { signature, started: performance.now(), clip: GAME_ASSETS.clips[exploding ? "explodingfrogs" : stateNames[placement]] };
        animationStates.set(key, state);
      }
      const canvas = document.createElement("canvas");
      // The animation PNGs include a 300px (three-cell) drawing area.
      // Keep the same pixels per grid unit, but allow the entire frame to render.
      canvas.width = canvas.height = 600;
      canvas.className = placement === "S" && !exploding ? "connector-tile single-frog" : "connector-tile";
      cell.classList.add("connected-cell");
      if (exploding) cell.classList.add("pop");
      state.canvas = canvas;
      state.cell = cell;
      state.frog = frog;
      state.tile = GAME_ASSETS.tiles[placement];
      cell.append(canvas);
    }
    cell.addEventListener("click", () => dropFrog(col));
    boardElement.append(cell);
  }
  for (const key of animationStates.keys()) if (!activeKeys.has(key)) animationStates.delete(key);
  paintFrogs(performance.now());
  const connections = FrogLogic.connections(board);
  const hasNewConnection = [...connections].some(pair => !previousConnections.has(pair));
  previousConnections = connections;
  if (hasNewConnection && !popping.length) playSound("connect");
}

function paintFrogs(now) {
  for (const state of animationStates.values()) {
    const elapsed = (now - state.started) / 1000;
    const animating = state.clip && (state.clip.loop || elapsed < state.clip.duration || state.signature.endsWith("explodingfrogs"));
    if (animating) state.cell.classList.add("connection-animating");
    else state.cell.classList.remove("connection-animating");
    const sprite = animating
      ? FrogLogic.frameAt(state.clip, elapsed) : state.tile;
    paintSprite(state.canvas, sprite, state.frog);
  }
}
function animateDirectionalConnections(now) {
  paintFrogs(now);
  paintCloud(now);
  requestAnimationFrame(animateDirectionalConnections);
}

async function resetGame() {
  saveRun();
  runSaved = false;
  gameGeneration += 1;
  const generation = gameGeneration;
  animationStates.clear();
  cloudAnimation = null;
  evilCloud.src = idleCloudSource;
  cloudCarrier.style.left = "50%";
  previousConnections.clear();
  if (typeof window !== "undefined" && !gameShell.hidden) {
    const space = gridSpace();
    const layout = GridLayout.choose(space.width, space.height);
    COLS = layout.cols;
    ROWS = layout.rows;
  }
  board = Array.from({ length: ROWS }, () => Array(COLS).fill(null));
  score = 0;
  level = 0;
  gameId = null;
  nextFrog = null;
  locked = true;
  updateHud();
  renderBoard();
  renderNextFrog();
  setMessage("Preparing the pond…");
  scoreBreakdown.textContent = "Each frog = 10 points · chain reactions multiply your points.";
  RetroDigits.render(comboElement, ScoreStore.bestCombo());
  fitBoard();

  try {
    const state = await gameApi("/api/games", {
      method: "POST",
      body: JSON.stringify({ rows: ROWS, columns: COLS }),
    });
    if (generation !== gameGeneration) return;
    gameId = state.id;
    board = boardFromApi(state.board);
    score = state.score;
    level = state.level;
    nextFrog = frogFromColor(state.nextFrog);
    locked = false;
    updateHud();
    renderNextFrog();
    renderBoard();
    setMessage("Click a column to drop a frog.");
  } catch (error) {
    if (generation !== gameGeneration) return;
    setMessage(`The game server could not start: ${error.message}`);
  }
}

function wait(milliseconds) { return new Promise((resolve) => setTimeout(resolve, milliseconds)); }

async function animateCloudDrop(row, col, frog) {
  const generation = gameGeneration;
  const horizontalPosition = ((col + .5) / COLS) * 100;
  const moveDuration = Math.max(430, GAME_ASSETS.cloudEat.duration * 1000);
  cloudCarrier.style.transitionDuration = `${moveDuration}ms`;
  cloudAnimation = { started: performance.now() };
  paintCloud(performance.now());
  cloudCarrier.style.left = `${horizontalPosition}%`;
  await wait(moveDuration + 15);
  if (generation !== gameGeneration) return;
  cloudAnimation = null;
  evilCloud.src = idleCloudSource;

  // Measure the real cell after any resize, rather than assuming a fixed board.
  const stageBounds = boardStage.getBoundingClientRect();
  const cloudBounds = evilCloud.getBoundingClientRect();
  const target = boardElement.children[row * COLS + col].getBoundingClientRect();
  fallingFrog.style.width = `${target.width}px`;
  fallingFrog.style.height = `${target.height}px`;
  const releaseTop = cloudBounds.top - stageBounds.top + cloudBounds.height * .6;
  fallingFrog.style.top = `${releaseTop}px`;
  const targetDistance = target.top - stageBounds.top - releaseTop;
  fallingFrog.replaceChildren();
  const sprite = GAME_ASSETS.tiles.S;
  const image = spriteImage(sprite.src);
  if (!image.complete) await image.decode().catch(() => {});
  const canvas = document.createElement("canvas");
  canvas.width = canvas.height = 200;
  paintSprite(canvas, sprite, frog);
  fallingFrog.append(canvas);
  fallingFrog.style.left = `${horizontalPosition}%`;
  fallingFrog.style.setProperty("--fall-distance", `${targetDistance}px`);
  fallingFrog.classList.remove("is-falling");
  void fallingFrog.offsetWidth;
  fallingFrog.classList.add("is-falling");
  cloudCarrier.classList.add("is-dropping");
  playSound("drop");
  await wait(590);
  if (generation !== gameGeneration) return;
  fallingFrog.classList.remove("is-falling");
  fallingFrog.replaceChildren();
}

async function dropFrog(col) {
  if (locked || !gameId) return;
  locked = true;
  const generation = gameGeneration;
  const previousLevel = level;

  try {
    const result = await gameApi(`/api/games/${gameId}/moves`, {
      method: "POST",
      body: JSON.stringify({ column: col }),
    });
    if (generation !== gameGeneration) return;

    const playedFrog = frogFromColor(result.frog);
    const row = result.placedAt.row;
    await animateCloudDrop(row, col, playedFrog);
    if (generation !== gameGeneration) return;
    board[row][col] = playedFrog;
    renderBoard({ row, col });
    await wait(260);

    for (const reaction of result.reactions) {
      if (generation !== gameGeneration) return;
      score += reaction.awarded;
      RetroDigits.render(comboElement, ScoreStore.updateCombo(reaction.awarded));
      updateHud();
      const matches = reaction.matches.map(position => ({ row: position.row, col: position.column }));
      scoreBreakdown.textContent = `${matches.length} frogs × 10 points × ${reaction.chain} chain = +${reaction.awarded.toLocaleString()} · Total: ${score.toLocaleString()}`;
      setMessage(`${matches.length} frogs hopped away! +${reaction.awarded}`);
      renderBoard(null, matches);
      playSound("clear");
      await wait(GAME_ASSETS.clips.explodingfrogs.duration * 1000);
      board = boardFromApi(reaction.boardAfterGravity);
      renderBoard();
    }

    const state = result.state;
    board = boardFromApi(state.board);
    score = state.score;
    level = state.level;
    nextFrog = frogFromColor(state.nextFrog);
    updateHud();
    renderNextFrog();

    if (state.status === "Completed") {
      saveRun();
      setMessage(`You completed FrogGame with ${score.toLocaleString()} points! Click New game to hop again.`);
      playSound("win");
      renderBoard();
      return;
    }
    if (state.status === "BoardFull") {
      saveRun();
      setMessage(`The pond is full! You earned ${score.toLocaleString()} points. Click New game to try again.`);
      playSound("error");
      renderBoard();
      return;
    }
    if (level > previousLevel) {
      setMessage(`Level ${level + 1}! More frog colours are hopping in.`);
      playSound("win");
    }

    locked = false;
    renderBoard();
  } catch (error) {
    if (generation !== gameGeneration) return;
    locked = false;
    setMessage(error.message);
    playSound("error");
    renderBoard();
  }
}

document.querySelector("#new-game").addEventListener("click", resetGame);
document.querySelector("#how-to-play").addEventListener("click", () => helpDialog.showModal());
document.querySelectorAll("#help-dialog .dialog-close").forEach((button) => button.addEventListener("click", () => helpDialog.close()));
document.querySelectorAll(".scores-close").forEach(button => button.addEventListener("click", () => scoresDialog.close()));
document.querySelector("#top-scores").addEventListener("click", showTopScores);
document.querySelector("#game-top-scores").addEventListener("click", showTopScores);
function closeIntroVideo() {
  introVideo.pause();
  introVideoPanel.hidden = true;
  watchVideoButton.hidden = false;
}
document.querySelector("#back-to-menu").addEventListener("click", closeIntroVideo);
document.querySelector("#main-menu").addEventListener("click", () => {
  backgroundMusic.pause();
  saveRun();
  gameGeneration += 1;
  locked = true;
  gameShell.hidden = true;
  intro.classList.remove("is-hidden");
  closeIntroVideo();
});
function startGame() {
  // Unlock sound directly in the click gesture, before any asynchronous work.
  playSound("start");
  closeIntroVideo();
  gameShell.hidden = false;
  resetGame();
  intro.classList.add("is-hidden");
  playBackgroundMusic();
  requestAnimationFrame(fitBoard);
}
watchVideoButton.addEventListener("click", () => {
  introVideoPanel.hidden = false;
  watchVideoButton.hidden = true;
  introVideo.muted = false;
  introVideo.defaultMuted = false;
  introVideo.volume = 1;
  introVideo.currentTime = 0;
  introVideo.play().catch(() => {
    document.querySelector(".intro-copy").textContent = "Press play on the video controls to hear the intro.";
  });
});
document.querySelector("#start-game").addEventListener("click", startGame);
introVideo.addEventListener("ended", () => document.querySelector("#start-game").focus());
soundToggle.addEventListener("click", () => {
  soundEnabled = soundPlaybackFailed ? true : !soundEnabled;
  soundPlaybackFailed = false;
  soundToggle.title = "";
  soundToggle.textContent = soundEnabled ? "♪" : "×";
  soundToggle.setAttribute("aria-pressed", String(soundEnabled));
  soundToggle.setAttribute("aria-label", soundEnabled ? "Mute sound" : "Enable sound");
  if (soundEnabled) {
    playSound("start");
    playBackgroundMusic();
  } else backgroundMusic.pause();
});

resetGame();
requestAnimationFrame(animateDirectionalConnections);

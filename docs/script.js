const API_BASE = "https://numbertowords-8z8i.onrender.com";

const numberInput = document.getElementById("numberInput");
const apiBaseDisplay = document.getElementById("apiBaseDisplay");
const wordsOutput = document.getElementById("wordsOutput");
const errorOutput = document.getElementById("errorOutput");

apiBaseDisplay.textContent = API_BASE;

let debounceTimer = null;

numberInput.addEventListener("input", () => {
  clearTimeout(debounceTimer);
  debounceTimer = setTimeout(() => handleInput(numberInput.value.trim()), 250);
});

function handleInput(rawValue) {
  if (rawValue === "") {
    showIdle();
    return;
  }
  convertNumber(rawValue);
}

async function convertNumber(rawValue) {
  try {
    const response = await fetch(
      `${API_BASE}/api/convert?number=${encodeURIComponent(rawValue)}`
    );
    const data = await response.json();

    if (!response.ok) {
      showError(data.error || "Something went wrong converting that number.");
      return;
    }

    showResult(data.words);
  } catch (err) {
    showError(
      `Couldn't reach the API at ${API_BASE}. Is it deployed and running?`
    );
  }
}

function showIdle() {
  wordsOutput.textContent = "Waiting for a number.";
  wordsOutput.hidden = false;
  errorOutput.hidden = true;
}

function showError(message) {
  errorOutput.textContent = message;
  errorOutput.hidden = false;
  wordsOutput.hidden = true;
}

function showResult(words) {
  errorOutput.hidden = true;
  wordsOutput.hidden = false;
  wordsOutput.textContent = words;
}
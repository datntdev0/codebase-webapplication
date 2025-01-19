// Namespace initialization with optional chaining
abp = globalThis.abp ??= {};

(() => {
  // Initialize UI namespace
  abp.ui ??= {};

  // Constants
  const SPINNER_SELECTOR = '#main-spinner';
  const HIDDEN_CLASS = 'd-none';

  /**
   * Cache the spinner element to avoid repeated DOM queries
   * @type {HTMLElement|null}
   */
  let spinnerElement = null;

  /**
   * Gets the spinner element, caching it for future use
   * @returns {HTMLElement|null}
   * @throws {Error} If spinner element is not found
   */
  const getSpinnerElement = () => {
    if (!spinnerElement) {
      spinnerElement = document.querySelector(SPINNER_SELECTOR);
      
      if (!spinnerElement) {
        throw new Error(`Spinner element "${SPINNER_SELECTOR}" not found in the DOM`);
      }
    }
    return spinnerElement;
  };

  /**
   * Shows the loading spinner
   * @throws {Error} If spinner element is not found
   */
  abp.ui.setBusy = () => {
    getSpinnerElement().classList.remove(HIDDEN_CLASS);
  };

  /**
   * Hides the loading spinner
   * @throws {Error} If spinner element is not found
   */
  abp.ui.clearBusy = () => {
    getSpinnerElement().classList.add(HIDDEN_CLASS);
  };
})();

'use strict';

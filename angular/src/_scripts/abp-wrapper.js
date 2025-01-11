// Namespace initialization with optional chaining
abp = globalThis.abp ??= {};

// IIFE with early return if SweetAlert2 is not available
(() => {
  if (typeof Swal === 'undefined') {
    console.warn('SweetAlert2 is required but not loaded');
    return;
  }

  // Constants
  const DEFAULT_TOAST_DURATION = 3000;
  const TOAST_POSITION = 'bottom-end';
  
  const NOTIFICATION_STYLES = {
    success: {
      background: '#34bfa3',
      icon: 'fas fa-check-circle'
    },
    info: {
      background: '#36a3f7',
      icon: 'fas fa-info-circle'
    },
    warning: {
      background: '#ffb822',
      icon: 'fas fa-exclamation-triangle'
    },
    error: {
      background: '#f4516c',
      icon: 'fas fa-exclamation-circle'
    }
  };

  // Initialize message and notify namespaces
  abp.message ??= {};
  abp.notify ??= {};

  /**
   * Shows a modal message dialog
   * @param {string} type - Message type (info/success/warning/error)
   * @param {string} message - Message content
   * @param {string} [title] - Dialog title
   * @param {boolean} [isHtml=false] - Whether message contains HTML
   * @param {Object} [options={}] - Additional SweetAlert2 options
   * @returns {Promise} SweetAlert2 promise
   */
  const showMessage = (type, message, title, isHtml = false, options = {}) => {
    // If only title is provided, use it as message
    if (!message) {
      message = title;
      title = undefined;
    }

    return Swal.fire({
      title,
      icon: type,
      confirmButtonText: options.confirmButtonText ?? abp.localization.abpWeb('Ok'),
      ...(isHtml ? { html: message } : { text: message }),
      ...options
    });
  };

  // Register message functions
  ['info', 'success', 'warn', 'error'].forEach(type => {
    abp.message[type] = (message, title, isHtml, options) => 
      showMessage(type === 'warn' ? 'warning' : type, message, title, isHtml, options);
  });

  /**
   * Shows a confirmation dialog
   * @param {string} message - Confirmation message
   * @param {(string|Function)} [titleOrCallback] - Title string or callback function
   * @param {Function} [callback] - Callback function
   * @param {boolean} [isHtml=false] - Whether message contains HTML
   * @param {Object} [options={}] - Additional SweetAlert2 options
   * @returns {Promise} SweetAlert2 promise
   */
  abp.message.confirm = (message, titleOrCallback, callback, isHtml = false, options = {}) => {
    const title = typeof titleOrCallback === 'function' ? undefined : titleOrCallback;
    const finalCallback = typeof titleOrCallback === 'function' ? titleOrCallback : callback;

    const confirmOptions = {
      title: title ?? abp.localization.abpWeb('AreYouSure'),
      icon: 'warning',
      confirmButtonText: options.confirmButtonText ?? abp.localization.abpWeb('Yes'),
      cancelButtonText: options.cancelButtonText ?? abp.localization.abpWeb('Cancel'),
      showCancelButton: true,
      ...(isHtml ? { html: message } : { text: message }),
      ...options
    };

    return Swal.fire(confirmOptions)
      .then(result => finalCallback?.(result.value));
  };

  // Initialize toast mixin
  const Toast = Swal.mixin({
    toast: true,
    position: TOAST_POSITION,
    showConfirmButton: false,
    timer: DEFAULT_TOAST_DURATION
  });

  /**
   * Shows a toast notification
   * @param {string} type - Notification type
   * @param {string} message - Notification message
   * @param {string} [title] - Notification title
   * @param {Object} [options={}] - Additional options
   */
  const showNotification = (type, message, title, options = {}) => {
    const style = NOTIFICATION_STYLES[type];
    const icon = options.customClass?.icon ? 
      `<i class="mr-2 text-light ${options.customClass.icon}"></i>` : '';
    
    const titleHtml = title ? 
      `${icon}<span class="text-light">${title}</span>` : '';
    
    const messageHtml = `${!title ? icon : ''}<span class="text-light">${message}</span>`;

    Toast.fire({
      background: style.background,
      customClass: {
        icon: style.icon
      },
      ...options,
      title: titleHtml || undefined,
      html: messageHtml
    });
  };

  // Register notification functions
  ['success', 'info', 'warn', 'error'].forEach(type => {
    abp.notify[type] = (message, title, options = {}) => {
      showNotification(type, message, title, {
        ...NOTIFICATION_STYLES[type],
        ...options
      });
    };
  });
})();

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

"use strict";

var abp = abp || {};

(function () {

  abp.ui.setBusy = function () {
    document.querySelector("#main-spinner").classList.remove("d-none");
  };

  abp.ui.clearBusy = function () {
    document.querySelector("#main-spinner").classList.add("d-none");
  };
})();
let tabindex = 1;

export function HandleTabFocus() {
  document.addEventListener("keydown", (event) => {
    if (event.keyCode === 9) {
      event.preventDefault();
      switch (tabindex) {
        case 1:
          document.getElementById("btn-previous").focus();
          tabindex = 2;
          break;
        case 2:
          document.getElementById("btn-next").focus();
          tabindex = 3;
          break;
        case 3:
          document.getElementById("lb-body").focus();
          tabindex = 1;
          break;
      }
    }
  })
}

export function SetTabIndex(x) {
  tabindex = x;
}
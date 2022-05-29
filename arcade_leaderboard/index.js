import { GetMaxPage, SortAllUsers } from "./modules/data.mjs";
import { HandleTabFocus, SetTabIndex } from "./modules/focus.mjs";

class Spinner {
  static Show() {
    const ele = document.getElementById("spinner");
    ele.style.visibility = "visible";
  }

  static Hide() {
    const ele = document.getElementById("spinner");
    ele.style.visibility = "hidden";
  }
}

window.LoadData = () => {
  return new Promise((res, rej) => {
    if (window.PAGE > window.PAGES.length) rej();
    res(window.PAGES[window.PAGE - 1]);
  });
}

window.InitializePage = () => {
  Spinner.Show();
  const tableBody = document.getElementById('lb-body');
  tableBody.innerHTML = "";

  window.LoadData()
    .then(dataArr => {
      for (let i = 0; i < dataArr.length; i++) {
        const tableBody = document.getElementById('lb-body');
        const tr = document.createElement('tr');

        const place = '#' + ((i + 1) + ((window.PAGE - 1) * 24));
        const th = document.createElement('th');
        th.scope = "row";
        // top 3 players
        if (i < 3 && window.PAGE == 1) {
          th.style.color = "#477bff";
          const h = document.createElement('h' + (i + 1));
          h.innerText = place;
          th.appendChild(h);
        } else th.innerText = place;

        const usernameField = document.createElement('td');
        usernameField.innerText = dataArr[i].username;

        const balanceField = document.createElement('td');
        balanceField.innerText = '$' + dataArr[i].balance;

        tr.appendChild(th);
        tr.appendChild(usernameField);
        tr.appendChild(balanceField);
        tableBody.appendChild(tr);
      }
      Spinner.Hide();
    });
  
  document.getElementById('lb-body').focus();
  SetTabIndex(1);
}

window.PAGE = 1;

window.UpdatePageLabel = (mode) => {
  if (mode != "add" && mode != "sub") throw new Error("mode must be equal to \"add\" or \"sub\"");
  const ele = document.getElementById('page-lbl');
  const txt = ele.innerText;
  const cnum = parseInt(txt.replace("Page ", ""));
  if ((cnum === 1 && mode === "sub") || (cnum === window.MAX_PAGE && mode === "add")) return; 
  window.PAGE = cnum + (mode === "add" ? 1 : -1);
  ele.innerText = `Page ${window.PAGE}`;
  window.InitializePage();
}

class Buttons  {
  static PreviousPage() {
    window.UpdatePageLabel('sub');
  }

  static NextPage() {
    window.UpdatePageLabel('add');
  }
}

window.InitializeButtonEventListeners = () => {
  document.addEventListener("keydown", event => {
    if (event.keyCode === 37)
      Buttons.PreviousPage();
  });

  document.addEventListener("keydown", event => {
    if (event.keyCode === 39)
      Buttons.NextPage();
  });

  document.getElementById("btn-previous").addEventListener("click", Buttons.PreviousPage);
  document.getElementById("btn-next").addEventListener("click", Buttons.NextPage);
}

window.onload = async() => {
  Spinner.Show();
  HandleTabFocus();
  window.MAX_PAGE = await GetMaxPage();
  window.PAGES = await SortAllUsers();
  window.InitializeButtonEventListeners();
  window.InitializePage();
}
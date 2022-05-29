function GetAllData() {
  return fetch("https://arcadeapp.fireapis.com/userdata/all?page_size=100000000", {
    method: "GET",
    headers: {
      "X-API-ID": 466,
      "X-CLIENT-TOKEN": "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJhcGlfa2V5IjoiMDAxYTJmMGItMTY0ZC00MjMzLWIzYTYtNmMzZTYwMTE2ODgzIiwidGVuYW50X2lkIjo1NzgsImp0aV9rZXkiOiIxMzQyZmFlYS1kYmViLTExZWMtYWQ1ZS0wYTU4YTlmZWFjMDIifQ.N0JQ6fafjHfnLvKdgX4_NqUFrY6dQ5Ui0dZp8q3PEOQ",
      "Content-Type": "application/json"
    }
  })
    .then(response => response.json())
    .then(response => response.data);
}

export function GetMaxPage() {
  return GetAllData()
    .then(data => { return Math.ceil(data.length / 25) });
}

export function SortAllUsers() {
  // sort user objects by balance in descending order
  return GetAllData().then(data => {

    return data.sort((a, b) => {
      return a.balance - b.balance;
    }).reverse();

  }).then(data => {
    let pages = [];
    let page = [];

    for (let i = 0; i < data.length; i++) {
      // 25 users on each page
      if ((i + 1) % 26 === 0) {
        pages.push(page);
        page = [];
      } else page.push(data[i]);
    }

    if (page.length !== 0)
      pages.push(page);

    return pages;
  });
}
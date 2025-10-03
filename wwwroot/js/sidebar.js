document.addEventListener("DOMContentLoaded", async () => {
    const response = await fetch("/api/menu");
    const menus = await response.json();

    const menuList = document.getElementById("menuList");
    const parents = menus.filter(m => m.parentId === null);

    parents.forEach(parent => {
        const li = document.createElement("li");
        li.classList.add("nav-item", "mb-2");

        const a = document.createElement("a");
        a.href = parent.url || "#";
        a.classList.add("nav-link", "text-white");
        a.innerHTML = `<i class="${parent.icon}"></i> ${parent.title}`;

        li.appendChild(a);

        // 子選單
        const children = menus.filter(m => m.parentId === parent.id);
        if (children.length > 0) {
            const ul = document.createElement("ul");
            ul.classList.add("nav", "flex-column", "ms-3");

            children.forEach(child => {
                const childLi = document.createElement("li");
                const childA = document.createElement("a");
                childA.href = child.url || "#";
                childA.classList.add("nav-link", "text-white");
                childA.innerHTML = `<i class="${child.icon}"></i> ${child.title}`;

                childLi.appendChild(childA);
                ul.appendChild(childLi);
            });

            li.appendChild(ul);
        }

        menuList.appendChild(li);
    });
});

document.addEventListener('DOMContentLoaded', () => {
    const form = document.getElementById('searchForm');
    const searchedElement = document.getElementById('element');
    const idInput  = document.getElementById('id');
    const btn = document.getElementById('searchButton');
    const list = document.getElementById('list');

    searchedElement.addEventListener('input', () => {
        const match = Array.from(list.options).find(opt => opt.value === searchedElement.value);

        if (match) {
            idInput.value = match.dataset.id;
            btn.disabled = false;
        }
        else {
            idInput.value = '';
            btn.disabled = true;
        }
    });
    
    form.addEventListener('submit', e => {
        if (!idInput.value) {
            e.preventDefault();
            searchedElement.reportValidity();
        }
    });
});

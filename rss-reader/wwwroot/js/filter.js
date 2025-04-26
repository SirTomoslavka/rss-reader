document.addEventListener('DOMContentLoaded', () => {
    const form = document.getElementById('dateFilterForm');
    const dateFrom  = document.getElementById('fromDate');
    const dateTo  = document.getElementById('toDate');
    const btn = document.getElementById('filterButton');

    function validateDates() {
        const fromVal = dateFrom.value;
        const toVal   = dateTo.value;
        
        if (!fromVal || !toVal) {
            btn.disabled = true;
            return;
        }
        
        const fromDateObj = new Date(fromVal);
        const toDateObj   = new Date(toVal);
        
        const today = new Date();
        today.setHours(0,0,0,0);
        
        const isFromInPast    = fromDateObj < today;
        const isToAfterFrom   = toDateObj > fromDateObj;

        btn.disabled = !(isFromInPast && isToAfterFrom);
    }
    
    dateFrom.addEventListener('change', validateDates);
    dateTo.addEventListener('change',   validateDates);
    
    validateDates();

    form.addEventListener('submit', e => {
        if (btn.disabled) {
            e.preventDefault();
        }
    });
});

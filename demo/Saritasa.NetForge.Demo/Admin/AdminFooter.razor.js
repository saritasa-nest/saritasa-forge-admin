function setVisitsCount () {
    let visitsCount = localStorage.getItem('visitsCount') ?? 0;
    visitsCount++;
    localStorage.setItem('visitsCount', visitsCount);

    const visits = document.getElementById('visits');
    visits.textContent = visitsCount;
}
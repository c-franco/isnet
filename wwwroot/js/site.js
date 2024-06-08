let currentPage = 1;

document.addEventListener('DOMContentLoaded', function () {
    searchSongs();
    fetchGenres();
});

async function fetchGenres() {
    const response = await fetch(`/Index?handler=Genres`);
    const genres = await response.json();
    const genreOptionsContainer = document.getElementById('genreOptions');

    genres.forEach(genre => {
        const label = document.createElement('label');
        label.innerHTML = `<input type="checkbox" value="${genre.key}" class="genre-checkbox" onchange="searchSongs(1)"> ${genre.value}`;
        genreOptionsContainer.appendChild(label);
    });
}

async function searchSongs(newPage) {
    const query = document.getElementById('searchBox').value.trim();
    const pageSize = document.getElementById('pageSize').value;
    const startYear = document.getElementById('startYear').value;
    const endYear = document.getElementById('endYear').value;
    const genres = Array.from(document.querySelectorAll('.genre-checkbox:checked')).map(cb => cb.value);

    currentPage = newPage;
    let response;

    if (query !== '') {
        let url = `/Index?handler=SearchSongs&query=${query}&currentPage=${currentPage}&pageSize=${pageSize}`;
        if (genres.length > 0) url += `&genres=${genres}`;
        if (startYear) url += `&startYear=${startYear}`;
        if (endYear) url += `&endYear=${endYear}`;
        response = await fetch(url);
    } else {
        response = await fetch(`/Index?handler=TrendingSongs&pageSize=${pageSize}`);
    }

    const resultsContainer = document.getElementById('results-container');
    if (resultsContainer) {
        if (pageSize == 15 || pageSize == 20) {
            resultsContainer.classList.add('padded-results');
        } else {
            resultsContainer.classList.remove('padded-results');
        }
    }

    const results = await response.json();
    const resultsList = document.getElementById('results');
    const noResultsMessage = document.getElementById('noResultsMessage');
    resultsList.innerHTML = '';

    if (results.length === 0) {
        noResultsMessage.style.display = 'block';
    } else {
        noResultsMessage.style.display = 'none';
        results.forEach(song => {
            const li = document.createElement('li');
            li.innerHTML = `<div class="song-info">
                                        <svg class="play-icon" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" class="lucide lucide-music-2"><circle cx="8" cy="18" r="4"/><path d="M12 18V2l7 4"/></svg>
                                        <div class="song-details">
                                            <span class="track-name">${song.track_Name}</span>
                                            <span class="artist-name">${song.artist_Name}</span>
                                        </div>
                                    </div>
                                    <span class="duration">${song.duration_ms}</span>`;
            resultsList.appendChild(li);
        });
    }

    document.getElementById('prevPage').disabled = currentPage <= 1 || query === '';
    document.getElementById('nextPage').disabled = results.length < pageSize || currentPage > 5 || query === '';
}

function changePage(delta) {
    currentPage += delta;
    searchSongs(currentPage);
}

function isNumberKey(evt) {
    var charCode = (evt.which) ? evt.which : evt.keyCode
    if (charCode > 31 && (charCode < 48 || charCode > 57))
        return false;
    return true;
}
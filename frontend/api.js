const API_BASE_URL = "http://localhost:5123/api/betting";

console.log("API URL:", API_BASE_URL);

async function getBalance() {
    const response = await fetch(`${API_BASE_URL}/balance`);
    const data = await response.json();
    return data.balance;
}

async function getMatches() {
    const response = await fetch(`${API_BASE_URL}/matches`);
    return await response.json();
}

async function getMatchesByLeague(league) {
    const response = await fetch(`${API_BASE_URL}/matches/league/${encodeURIComponent(league)}`);
    return await response.json();
}

async function placeBet(matchId, amount) {
    const response = await fetch(`${API_BASE_URL}/bet`, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ matchId, betType: "home", amount })
    });
    
    if (!response.ok) {
        const error = await response.json();
        throw new Error(error.message || "Ошибка при ставке");
    }
    return await response.json();
}

async function completeMatch(matchId, homeScore, awayScore) {
    const response = await fetch(`${API_BASE_URL}/complete-match/${matchId}`, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ homeScore, awayScore })
    });
    return await response.json();
}

async function resetBalance() {
    const response = await fetch(`${API_BASE_URL}/reset-balance`, {
        method: "POST"
    });
    return await response.json();
}

window.API = {
    getBalance,
    getMatches,
    getMatchesByLeague,
    placeBet,
    completeMatch,
    resetBalance
};
const balanceAmount = document.getElementById("balanceAmount");
const matchesList = document.getElementById("matchesList");
const loadingText = document.getElementById("loadingText");

let currentBalance = 0;
let allMatches = [];
let currentFilter = "all";
let currentMatch = null;

const modal = document.getElementById("betModal");
const closeBtn = document.getElementsByClassName("close")[0];

async function loadBalance() {
    try {
        const balance = await API.getBalance();
        currentBalance = balance;
        balanceAmount.textContent = Math.floor(currentBalance);
    } catch (error) {
        console.error("Ошибка:", error);
        balanceAmount.textContent = "10000";
        currentBalance = 10000;
    }
}

async function loadMatches() {
    try {
        loadingText.style.display = "block";
        
        let matches;
        if (currentFilter === "all") {
            matches = await API.getMatches();
        } else {
            matches = await API.getMatchesByLeague(currentFilter);
        }
        
        allMatches = matches;
        renderMatches();
        loadingText.style.display = "none";
    } catch (error) {
        console.error("Ошибка:", error);
        loadingText.style.display = "none";
        matchesList.innerHTML = '<div class="empty-text">❌ Ошибка загрузки матчей</div>';
    }
}

function escapeHtml(str) {
    if (!str) return "";
    return str.replace(/[&<>]/g, function(m) {
        if (m === "&") return "&amp;";
        if (m === "<") return "&lt;";
        if (m === ">") return "&gt;";
        return m;
    });
}

function createMatchHTML(match) {
    const isCompleted = match.isCompleted;
    const canBet = !isCompleted && currentBalance >= 10;
    
    let resultHtml = "";
    if (isCompleted) {
        resultHtml = `<div class="match-result">🏆 Счет: ${match.homeScore} : ${match.awayScore}</div>`;
    }
    
    const completeBtn = !isCompleted ? `
        <button class="complete-match-btn" data-match-id="${match.id}" data-match-home="${escapeHtml(match.homeTeam)}" data-match-away="${escapeHtml(match.awayTeam)}">
            🏆 Завершить матч
        </button>
    ` : '';
    
    return `
        <li class="match-card ${isCompleted ? 'completed' : ''}">
            <div class="match-header">
                <div class="match-league">${match.league === "Champions League" ? "🏆 Лига Чемпионов" : "🇪🇸 Ла Лига"}</div>
                <div class="match-teams">
                     <strong>${escapeHtml(match.homeTeam)} (Хозяева)</strong> vs  ${escapeHtml(match.awayTeam)} (Гости)
                </div>
            </div>
            <div class="match-body">
                <div class="match-odds">
                    <div class="odd-item" style="background: #f0fff4;">
                        <div class="odd-label">⭐ Победа хозяев</div>
                        <div class="odd-value" style="color: #48bb78;">${match.homeOdds}</div>
                    </div>
                    <div class="odd-item">
                        <div class="odd-label">🤝 Ничья</div>
                        <div class="odd-value">${match.drawOdds}</div>
                    </div>
                    <div class="odd-item">
                        <div class="odd-label"> Победа гостей</div>
                        <div class="odd-value">${match.awayOdds}</div>
                    </div>
                </div>
                ${resultHtml}
                <div class="bet-buttons">
                    <button class="bet-btn" data-match-id="${match.id}" ${!canBet ? 'disabled' : ''}>
                        ${canBet ? '🎲 Сделать ставку на победу хозяев' : (isCompleted ? '📊 Матч завершен' : '💰 Недостаточно средств')}
                    </button>
                </div>
                ${completeBtn}
            </div>
        </li>
    `;
}

function renderMatches() {
    if (!allMatches || allMatches.length === 0) {
        matchesList.innerHTML = '<div class="empty-text">Нет матчей</div>';
        return;
    }
    
    matchesList.innerHTML = allMatches.map(match => createMatchHTML(match)).join("");
    
    allMatches.forEach(match => {
        if (!match.isCompleted && currentBalance >= 10) {
            const betBtn = document.querySelector(`.bet-btn[data-match-id="${match.id}"]`);
            if (betBtn) {
                betBtn.onclick = () => openBetModal(match);
            }
        }
        
        const completeBtn = document.querySelector(`.complete-match-btn[data-match-id="${match.id}"]`);
        if (completeBtn) {
            completeBtn.onclick = () => completeMatch(match.id, match.homeTeam, match.awayTeam);
        }
    });
}

function openBetModal(match) {
    currentMatch = match;
    document.getElementById("modalMatchInfo").innerHTML = `
        <strong>${escapeHtml(match.homeTeam)}</strong> vs <strong>${escapeHtml(match.awayTeam)}</strong><br>
        Коэффициент на победу хозяев: ${match.homeOdds}
    `;
    document.getElementById("betAmount").value = "";
    document.getElementById("betError").textContent = "";
    modal.style.display = "block";
}

async function confirmBet() {
    const amount = parseInt(document.getElementById("betAmount").value);
    
    if (!amount || isNaN(amount)) {
        document.getElementById("betError").textContent = "Введите сумму";
        return;
    }
    
    if (amount < 10) {
        document.getElementById("betError").textContent = "Минимум 10 монет";
        return;
    }
    
    if (amount > currentBalance) {
        document.getElementById("betError").textContent = `Недостаточно средств. Баланс: ${currentBalance}`;
        return;
    }
    
    const confirmBtn = document.getElementById("confirmBetBtn");
    confirmBtn.disabled = true;
    confirmBtn.textContent = "Обработка...";
    
    try {
        const result = await API.placeBet(currentMatch.id, amount);
        alert(`✅ Ставка принята!\nСумма: ${amount}\nКоэффициент: ${currentMatch.homeOdds}\nНовый баланс: ${result.newBalance}`);
        await loadBalance();
        await loadMatches();
        modal.style.display = "none";
    } catch (error) {
        document.getElementById("betError").textContent = error.message;
    } finally {
        confirmBtn.disabled = false;
        confirmBtn.textContent = "✅ Подтвердить ставку";
    }
}

async function completeMatch(matchId, homeTeam, awayTeam) {
    const homeScore = parseInt(prompt(`Счет ${homeTeam} (хозяева):`, "2"));
    const awayScore = parseInt(prompt(`Счет ${awayTeam} (гости):`, "1"));
    
    if (isNaN(homeScore) || isNaN(awayScore)) return;
    
    if (!confirm(`Завершить матч ${homeTeam} ${homeScore}:${awayScore} ${awayTeam}?`)) return;
    
    try {
        const result = await API.completeMatch(matchId, homeScore, awayScore);
        const winner = result.isHomeWin ? homeTeam : (awayScore > homeScore ? awayTeam : "Ничья");
        alert(`✅ Матч завершен!\n${homeTeam} ${result.homeScore}:${result.awayScore} ${awayTeam}\nПобедитель: ${winner}\nВыплачено: ${result.totalPayout} монет`);
        await loadBalance();
        await loadMatches();
    } catch (error) {
        alert("Ошибка: " + error.message);
    }
}

async function resetBalanceHandler() {
    if (confirm("Сбросить баланс до 10000 монет?")) {
        try {
            await API.resetBalance();
            await loadBalance();
            alert("✅ Баланс сброшен!");
        } catch (error) {
            alert("Ошибка: " + error.message);
        }
    }
}

function initEventListeners() {
    if (closeBtn) {
        closeBtn.onclick = () => modal.style.display = "none";
    }
    
    window.onclick = (event) => {
        if (event.target === modal) modal.style.display = "none";
    };
    
    document.getElementById("confirmBetBtn").addEventListener("click", confirmBet);
    document.getElementById("resetBalanceBtn").addEventListener("click", resetBalanceHandler);
    
    document.querySelectorAll(".filter-btn").forEach(btn => {
        btn.addEventListener("click", function() {
            document.querySelectorAll(".filter-btn").forEach(b => b.classList.remove("active"));
            this.classList.add("active");
            currentFilter = this.getAttribute("data-filter");
            loadMatches();
        });
    });
}

async function init() {
    initEventListeners();
    loadingText.textContent = "Загрузка...";
    await loadBalance();
    await loadMatches();
}

init();
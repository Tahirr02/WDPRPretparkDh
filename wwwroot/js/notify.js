"use strict";

const NotifyConnection = new signalR.HubConnectionBuilder().withUrl("/notifyhub").build();
const attractieId = document.getElementById('Id').value;

document.getElementById("AantalPlekkenVak").addEventListener("input", e => {
    let aantalPlekken = 0;
    if(e.target.value != ""){
        aantalPlekken = parseInt(e.target.value);
    }
    let prijs = parseInt(document.getElementById("AttractiePrijs").textContent) * aantalPlekken;
    document.getElementById("Prijs").textContent = prijs;
})

NotifyConnection.on("ReceiveReservatie", (beschikbaarPlekken) => {
    document.getElementById("plekkenInfo").textContent = beschikbaarPlekken;
    checkDisable(beschikbaarPlekken);
})

NotifyConnection.on("ReceiveLikes", (likes) => {
    document.getElementById("likes").textContent = likes;
    // document.getElementById("likeBtn").disabled = true;
})

NotifyConnection.on("LoadPlekken", (beschikbaarPlekken) => {
    checkDisable(beschikbaarPlekken);
})

NotifyConnection.start().then(function () {
    NotifyConnection.invoke('getAvailable', attractieId)
    let aantalPlekken = document.getElementById("AantalPlekkenVak").value
    NotifyConnection.invoke("Reserveer", attractieId, aantalPlekken).catch( (err) => {
        return console.error.apply(err.toString());
    })
}).catch(function (err) {
    return console.error(err.toString());
});

NotifyConnection.on("updateReservaties", () => {
    let aantalPlekken = document.getElementById("AantalPlekkenVak").value
    NotifyConnection.invoke("Reserveer", attractieId, aantalPlekken).catch( (err) => {
        return console.error.apply(err.toString());
    })
})

document.getElementById("likeBtn").addEventListener("click", () => {
    
    NotifyConnection.invoke("SendLike", attractieId).catch( (err) => {
        return console.error.apply(err.toString());
    })
})


function checkDisable(beschikbaarPlekken){
    if(beschikbaarPlekken == 0){
        document.getElementById("bookBtn").disabled = true;
        document.getElementById("AantalPlekkenVak").disabled = true;
        document.getElementById("TijdslotVak").disabled = true;
    }
}

// function checkDisable2()
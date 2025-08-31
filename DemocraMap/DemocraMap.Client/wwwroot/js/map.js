var map;
var indigenousLayers = [];
var migrantLayers = [];
var outreachLayers = [];

window.initMap = function () {

    map = L.map('map').setView([-25.2744, 133.7751], 4);

    L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
        attribution: '&copy; OpenStreetMap contributors'
    }).addTo(map);
};

window.updateMarkers = function (
    showIndigenous, showMigrants, showOutreach,
    indigenous, migrants, outreach
) {

    function clearLayers(arr) {
        arr.forEach(layer => map.removeLayer(layer));
        arr.length = 0;
    }

    clearLayers(indigenousLayers);
    clearLayers(migrantLayers);
    clearLayers(outreachLayers);

    // Indigenous base markers
    if (showIndigenous && Array.isArray(indigenous)) {
        indigenous.forEach(region => {
            var marker = L.marker([region.latitude, region.longitude])
                .addTo(map)
                .bindPopup(`<b>${region.region}</b><br/>Enrolment: ${region.enrolmentRate}`);
            indigenousLayers.push(marker);
        });
    }

    // Migrant base markers
    if (showMigrants && Array.isArray(migrants)) {
        migrants.forEach(region => {
            var marker = L.marker([region.latitude, region.longitude])
                .addTo(map)
                .bindPopup(`<b>${region.region}</b><br/>Enrolment: ${region.enrolmentRate}`);
            migrantLayers.push(marker);
        });
    }

    // Outreach circles — filtered by toggles
    if (showOutreach && Array.isArray(outreach)) {
        outreach.forEach(region => {
            let include = false;

            if ((showIndigenous && showMigrants) || (!showIndigenous && !showMigrants)) {
                include = true; // all outreach
            } else if (showIndigenous && !showMigrants && region.groupType === "Indigenous") {
                include = true;
            } else if (showMigrants && !showIndigenous && region.groupType === "Migrant") {
                include = true;
            }

            if (include) {
                let size = 8;
                if (region.targetGroupPercent) {
                    let num = parseFloat(region.targetGroupPercent);
                    if (!isNaN(num)) {
                        size = Math.max(6, Math.min(25, num / 2));
                    }
                }

                var circle = L.circleMarker([region.latitude, region.longitude], {
                    radius: size,
                    color: (region.groupType === "Indigenous" ? "darkred" : "darkorange"),
                    fillColor: (region.groupType === "Indigenous" ? "#e74c3c" : "#ffa500"),
                    fillOpacity: 0.6,
                    weight: 2
                })
                    .addTo(map)
                    .bindPopup(`<b>${region.region}</b><br/>Group: ${region.groupType}<br/>Target %: ${region.targetGroupPercent}`);

                outreachLayers.push(circle);
            }
        });
    }
};
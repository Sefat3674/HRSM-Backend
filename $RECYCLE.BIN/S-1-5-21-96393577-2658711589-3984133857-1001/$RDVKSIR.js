//const style = document.createElement('style');
//style.innerHTML = `
///* Full page split layout */
//#pageLayout {
//    display: flex;
//    height: 100vh;
//    background: #f5f7fa;
//}
///* Left Panel - 15% WIP List */
//#leftWipPanel {
//    width: 15%;
//    min-width: 280px;
//    background: white;
//    border-right: 1px solid #dee2e6;
//    box-shadow: 4px 0 15px rgba(0,0,0,0.1);
//    overflow-y: auto;
//    padding: 20px;
//}
//#leftWipPanel h2 {
//    font-size: 1.3rem;
//    color: #2c3e50;
//    margin-bottom: 16px;
//    text-align: center;
//}
///* WIP List Table */
//#wipListTable {
//    width: 100%;
//    border-collapse: collapse;
//    font-size: 0.95rem;
//}
//#wipListTable th {
//    background: #34495e;
//    color: white;
//    padding: 10px;
//    font-size: 0.9rem;
//}
//#wipListTable td {
//    padding: 10px;
//    border-bottom: 1px solid #ecf0f1;
//}
//#wipListTable button {
//    width: 100%;
//    padding: 10px;
//    font-size: 0.95rem;
//    border-radius: 8px;
//}
//.active-wip-row {
//    background: #e8f4fd !important;
//    font-weight: bold;
//    border-left: 5px solid #3498db;
//}
///* Right Panel - 85% */
//#rightGanttPanel {
//    width: 85%;
//    padding: 20px;
//    overflow: auto;
//}
//.blank-state {
//    display: flex;
//    align-items: center;
//    justify-content: center;
//    height: 100%;
//    font-size: 1.8rem;
//    color: #95a5a6;
//    text-align: center;
//}
///* Gantt Header */
//.gantt-header {
//    background: linear-gradient(135deg, #3498db, #2980b9);
//    color: white;
//    padding: 16px;
//    font-size: 1.4rem;
//    display: flex;
//    justify-content: space-between;
//    align-items: center;
//    border-radius: 12px 12px 0 0;
//    margin-bottom: 0;
//}
//.gantt-controls {
//    display: flex;
//    align-items: center;
//    gap: 12px;
//}

//#ganttViewToggle {
//    padding: 6px 12px;
//    border-radius: 8px;
//    border: none;
//    background: rgba(255,255,255,0.2);
//    color: black;
//    font-size: 0.95rem;
//}
//.gantt-header button {
//    background: rgba(255,255,255,0.2);
//    border: none;
//    color: white;
//    padding: 6px 12px;
//    border-radius: 8px;
//    cursor: pointer;
//    font-size: 0.95rem;
//}
///* Gantt Scroll */
//#ganttScrollContainer {
//    background: white;
//    border-radius: 0 0 12px 12px;
//    box-shadow: 0 10px 30px rgba(0,0,0,0.1);
//    overflow: auto;
//    padding: 20px;
//}
///* Gantt Table */
//#ganttTable {
//    width: max-content;
//    min-width: 100%;
//    border-collapse: separate;
//    border-spacing: 0;
//    font-size: 0.9rem;
//}
//#ganttTable th {
//    background: #ecf0f1;
//    color: #2c3e50;
//    font-weight: 600;
//    padding: 10px 8px;
//    border-bottom: 3px solid #3498db;
//    position: sticky;
//    top: 0;
//    z-index: 10;
//    min-width: 35px;
//}
//#ganttTable td:first-child, #ganttTable th:first-child {
//    position: sticky;
//    left: 0;
//    background: #f8f9fa !important;
//    min-width: 280px;
//    padding-left: 20px;
//    z-index: 9;
//    box-shadow: 6px 0 12px rgba(0,0,0,0.08);
//}
//#ganttTable tbody tr:nth-child(4n+1), #ganttTable tbody tr:nth-child(4n+2) { background-color: #f8fbff; }
//.gantt-scheduled {
//    min-width: 35px;
//    height: 28px;
//    position: relative;
//    border: 1px solid #d0d7de;
//}
//.gantt-scheduled.pending { background: linear-gradient(135deg, #fff3cd, #ffeaa7); }
//.gantt-scheduled.running { background: linear-gradient(135deg, #d1ecf1, #74b9ff); }
//.gantt-scheduled.completed { background: linear-gradient(135deg, #d4edda, #55a3ff); }
//.gantt-scheduled.pause { background: linear-gradient(135deg, #f8d7da, #ff7675); }
//.gantt-scheduled.occupied { color: #2c3e50; font-weight: 700; font-size: 12px; text-align: center; }
//.gantt-actual {
//    min-width: 35px;
//    height: 20px;
//    position: relative;
//    border: 1px solid #d0d7de;
//    background: #f8f9fa;
//}
//.actual-bar {
//    position: absolute;
//    top: 3px; bottom: 3px; left: 0; right: 0;
//    background: linear-gradient(90deg, #00b894, #00a085);
//    border-radius: 6px;
//    border: 1px solid #00a085;
//    box-shadow: 0 3px 8px rgba(0,168,132,0.3);
//}
//.actual-bar.ongoing {
//    opacity: 0.8;
//    animation: pulse 2s infinite;
//}
//@keyframes pulse {
//    0% { opacity: 0.8; }
//    50% { opacity: 0.6; }
//    100% { opacity: 0.8; }
//}
//.gantt-today-line {
//    position: absolute; top: 0; bottom: 0; width: 4px;
//    background: #e74c3c; border-left: 4px dashed #e74c3c; z-index: 15;
//    box-shadow: 0 0 15px rgba(231,76,60,0.6);
//}
///* Tooltip */
//.gantt-scheduled.occupied:hover::after {
//    content: attr(data-tooltip);
//    position: absolute; bottom: 120%; left: 50%; transform: translateX(-50%);
//    background: rgba(0,0,0,0.92); color: #fff; padding: 12px 16px; border-radius: 10px;
//    font-size: 12px; line-height: 1.6; white-space: pre-line; z-index: 1000;
//    min-width: 300px; max-width: 420px; box-shadow: 0 12px 35px rgba(0,0,0,0.4);
//}
//.gantt-scheduled.occupied:hover::before {
//    content: ''; position: absolute; bottom: 120%; left: 50%; transform: translateX(-50%);
//    border: 10px solid transparent; border-top-color: rgba(0,0,0,0.92); z-index: 1000;
//}
//.task-label {
//    font-size: 14px;
//    color: #2c3e50;
//    display: flex;
//    flex-direction: column;
//    justify-content: center;
//}

//.gantt-actual.error {
//    background: #fff5f5;
//    border-color: #f1aeb5;
//}
//.actual-error-text {
//    color: #c1121f;
//    font-weight: 600;
//    font-size: 12px;
//    letter-spacing: 0.2px;
//}

//.actual-label {
//    font-size: 11px;
//    color: #7f8c8d;
//    font-style: italic;
//    padding-left: 20px;
//}
//`;


//document.head.appendChild(style);

//// Global state
//let ganttView = 'hour';
//let currentGrouped = null;
//let currentItemId = null;
//let currentItemName = null;
//let activeWipName = null;
//let pdfReady = false;

//$(document).ready(function () {
//    $("#itemSearchButton").off('click').on('click', function () {
//        loadItemTable($("#LocationIdSearch").val(), $("#BusinessUnitIdSearch").val(), $("#SectionIdSearch").val());
//    });

//    $(document).off('change', '#ganttViewToggle').on('change', '#ganttViewToggle', function () {
//        ganttView = $(this).val();
//        if (activeWipName && currentGrouped && currentGrouped[activeWipName]) {
//            renderGanttInRightPanel(activeWipName, currentGrouped[activeWipName]);
//        }
//    });

//    $(document).off('click', '.show-gantt-btn').on('click', '.show-gantt-btn', function () {
//        const wipName = $(this).data('wip');
//        activeWipName = wipName;
//        renderGanttInRightPanel(wipName, currentGrouped[wipName]);
//        highlightActiveRow(wipName);
//    });

//    $(document).off('click', '.download-pdf-btn').on('click', '.download-pdf-btn', function () {
//        if (!pdfReady) {
//            alert('PDF library is still loading. Please wait a few seconds and try again.');
//            return;
//        }
//        generatePDF(activeWipName);
//    });
//});

//(function waitForPDF() {
//    if (window.jspdf && window.jspdf.jsPDF && window.jspdf.jsPDF.autoTable) {
//        pdfReady = true;
//        $('.download-pdf-btn').prop('disabled', false);
//    } else {
//        setTimeout(waitForPDF, 500);
//    }
//})();

//function generatePDF(wipName) {
//    const { jsPDF } = window.jspdf;
//    const doc = new jsPDF('l', 'mm', 'a4');
//    wipName = wipName || 'Gantt';
//    doc.setFontSize(18);
//    doc.text(`Gantt Chart - WIP: ${wipName}`, 14, 20);
//    doc.setFontSize(12);
//    doc.text(`Generated on: ${new Date().toLocaleString()}`, 14, 30);
//    doc.autoTable({
//        html: '#ganttTable',
//        startY: 40,
//        theme: 'grid',
//        styles: { fontSize: 8, cellPadding: 3 },
//        headStyles: { fillColor: [52, 152, 219], textColor: 255, fontStyle: 'bold' },
//        alternateRowStyles: { fillColor: [248, 249, 250] },
//        columnStyles: { 0: { cellWidth: 60, fontStyle: 'bold' } },
//        margin: { top: 40, left: 10, right: 10 },
//        didDrawPage: (data) => {
//            doc.setFontSize(10);
//            doc.text(`Page ${data.pageNumber}`, 280, 200);
//        }
//    });
//    doc.save(`Gantt_${wipName.replace(/[^a-z0-9]/gi, '_')}_${new Date().toISOString().slice(0, 10)}.pdf`);
//}

//function loadItemTable(locationId, businessUnitId, sectionId) {
//    if ($.fn.DataTable.isDataTable("#itemTable")) $("#itemTable").DataTable().destroy();
//    const table = $("#itemTable").DataTable({
//        processing: true,
//        serverSide: true,
//        ajax: {
//            url: "/item/getall",
//            type: "POST",
//            data: { LocationId: locationId, BusinessUnitId: businessUnitId, SectionId: sectionId }
//        },
//        columns: [
//            { data: "itemName" },
//            { data: "section" },
//            { data: "itemConfigurationType" },
//            { data: "machineType" },
//            { data: "itemName" },
//            { data: "itemCode" },
//            { data: "sct" },
//            { data: "cavity" },
//            { data: "weight" },
//            {
//                data: null,
//                orderable: false,
//                render: function (row) {
//                    return `<button class="btn btn-primary btn-sm wip-btn" data-itemid="${row.id}" data-itemname="${row.itemName}">Show WIP</button>`;
//                }
//            }
//        ]
//    });
//}

//$(document).off('click', '.wip-btn').on('click', '.wip-btn', function () {
//    const itemId = $(this).data('itemid');
//    const itemName = $(this).data('itemname');
//    showWipListInLeftPanel(itemId, itemName);
//});

//function showWipListInLeftPanel(itemId, itemName) {
//    const leftPanel = $("#leftWipPanel").empty();
//    leftPanel.append(`<h2>WIPs for Item: ${itemName}</h2>`);
//    const listTable = $(`<table id="wipListTable"><thead><tr><th>WIP Name</th><th>Action</th></tr></thead><tbody></tbody></table>`);
//    leftPanel.append(listTable);
//    $("#rightGanttPanel").html('<div class="blank-state">Select a WIP and click "View Gantt" to see the chart</div>');
//    currentItemId = itemId;
//    currentItemName = itemName;
//    activeWipName = null;

//    $.post("/ToolRoomDashboard/getWIPGanttByItemId", { itemId }, function (tasks) {
//        if (!tasks || tasks.length === 0) {
//            listTable.find("tbody").html("<tr><td colspan='2'>No WIPs found</td></tr>");
//            return;
//        }
//        currentGrouped = {};
//        tasks.forEach(t => (currentGrouped[t.name] ||= []).push(t));
//        const tbody = listTable.find("tbody").empty();
//        Object.keys(currentGrouped).sort().forEach(wipName => {
//            tbody.append(`
//                <tr data-wip="${wipName}">
//                    <td><strong>${wipName}</strong></td>
//                    <td><button class="btn btn-success show-gantt-btn" data-wip="${wipName}">View Gantt</button></td>
//                </tr>
//            `);
//        });
//    });
//}

//function highlightActiveRow(wipName) {
//    $("#wipListTable tr").removeClass("active-wip-row");
//    $(`#wipListTable tr[data-wip="${wipName}"]`).addClass("active-wip-row");
//}

//function renderGanttInRightPanel(wipName, tasks) {
//    const rightPanel = $("#rightGanttPanel").empty();
//    if (!tasks || tasks.length === 0) {
//        rightPanel.html('<div class="blank-state">No tasks found for this WIP</div>');
//        return;
//    }

//    const header = $(`
//        <div class="gantt-header">
//            <span>Gantt Chart — ${wipName}</span>
//            <div class="gantt-controls">
//                <label>View:</label>
//                <select id="ganttViewToggle">
//                    <option value="hour">Hour View</option>
//                    <option value="day">Day View</option>
//                </select>
//                <button class="download-pdf-btn">Download PDF</button>
//            </div>
//        </div>
//    `);
//    rightPanel.append(header);
//    header.find("#ganttViewToggle").val(ganttView);

//    const scrollContainer = $('<div id="ganttScrollContainer"><div id="ganttContainer"></div></div>');
//    rightPanel.append(scrollContainer);
//    const container = scrollContainer.find("#ganttContainer");
//    const table = $(`<table id="ganttTable"><thead id="ganttHeader"></thead><tbody id="ganttBody"></tbody></table>`);
//    container.append(table);

//    const allDates = tasks.flatMap(t => [
//        new Date(t.startTime),
//        new Date(t.endTime),
//        t.actualStartTime ? new Date(t.actualStartTime) : null,
//        t.actualEndTime ? new Date(t.actualEndTime) : null
//    ]).filter(Boolean);

//    const minDate = new Date(Math.min(...allDates));
//    const maxDate = new Date(Math.max(...allDates));
//    const intervalMs = ganttView === 'hour' ? 30 * 60 * 1000 : 24 * 60 * 60 * 1000;
//    const totalCols = Math.ceil((maxDate - minDate) / intervalMs) + 1;

//    const headerRow = $("<tr></tr>").append("<th rowspan='2'>Task Details</th>");
//    for (let i = 0; i < totalCols; i++) {
//        const d = new Date(minDate.getTime() + i * intervalMs);
//        const label = ganttView === 'hour'
//            ? `${d.getDate().toString().padStart(2, '0')}/${(d.getMonth() + 1).toString().padStart(2, '0')} ${d.getHours().toString().padStart(2, '0')}:${d.getMinutes().toString().padStart(2, '0')}`
//            : `${d.getDate().toString().padStart(2, '0')}/${(d.getMonth() + 1).toString().padStart(2, '0')}/${d.getFullYear()}`;
//        headerRow.append(`<th>${label}</th>`);
//    }
//    table.find("#ganttHeader").append(headerRow);

//    const subHeader = $("<tr></tr>").append("<th>Actual Progress</th>");
//    for (let i = 0; i < totalCols; i++) subHeader.append("<th></th>");
//    table.find("#ganttHeader").append(subHeader);

//    tasks.sort((a, b) => new Date(a.startTime) - new Date(b.startTime));

//    tasks.forEach(t => {
//        if (!t.status || t.status === "Null") return;

//        const scheduledStart = new Date(t.startTime);
//        const scheduledEnd = new Date(t.endTime);
//        const sStartIdx = Math.max(0, Math.floor((scheduledStart - minDate) / intervalMs));
//        const sEndIdx = Math.min(totalCols - 1, Math.floor((scheduledEnd - minDate) / intervalMs));

//        const taskLabel = `${t.taskName || 'Task'}${t.machineName ? ' — ' + t.machineName : ''}<br><small style="color:#6c757d;">Status: ${t.status}${t.operatorName ? ' (' + t.operatorName + ')' : ''}</small>`;
//        const tooltip = `Task: ${t.taskName || ''}\nMachine: ${t.machineName || ''}\nOperator: ${t.operatorName || ''}\nScheduled: ${scheduledStart.toLocaleString()} → ${scheduledEnd.toLocaleString()}\nActual: ${t.actualStartTime ? new Date(t.actualStartTime).toLocaleString() : 'N/A'} → ${t.actualEndTime ? new Date(t.actualEndTime).toLocaleString() : 'N/A'}\nStatus: ${t.status}\nTotal Time: ${t.totalTime || 0} hrs`;

//        let statusClass = t.status.toLowerCase() === "running" ? "running" :
//            t.status.toLowerCase() === "completed" ? "completed" :
//                t.status.toLowerCase() === "pause" ? "pause" : "pending";

//        // Scheduled Row
//        const schRow = $("<tr></tr>").append(`<td class="task-label">${taskLabel}</td>`);
//        let i = 0;
//        while (i < totalCols) {
//            if (i < sStartIdx || i > sEndIdx) {
//                schRow.append("<td class='gantt-scheduled'></td>");
//                i++;
//            } else if (i === sStartIdx) {
//                const colspan = sEndIdx - sStartIdx + 1;
//                schRow.append(`<td class="gantt-scheduled occupied ${statusClass}" colspan="${colspan}" data-tooltip="${tooltip}">${t.status.toUpperCase()}</td>`);
//                i = sEndIdx + 1;
//            }
//        }
//        table.find("#ganttBody").append(schRow);
//        const actRow = $("<tr></tr>").append(`<td class="actual-label">Actual</td>`);

//        let actualStart = null;
//        let actualEnd = null;
//        let isOngoing = false;

//        if (t.actualStartTime) {
//            actualStart = new Date(t.actualStartTime);

//            if (t.actualEndTime) {
//                actualEnd = new Date(t.actualEndTime);
//            } else if (t.status && (t.status.toLowerCase() === "running" || t.status.toLowerCase() === "pause")) {
//                actualEnd = new Date(); // ongoing
//                isOngoing = true;
//            }
//        }

//        // If we have both times, validate sequence first
//        if (actualStart && actualEnd) {
//            if (actualEnd < actualStart) {
//                // Show a clear message instead of a bar
//                actRow.append(`
//          <td class="gantt-actual error" colspan="${totalCols}">
//            <span class="actual-error-text">Actual end time is before start time — please verify data.</span>
//          </td>
//        `);
//            } else {
//                // Draw bar with pixel offsets
//                const $headerCells = $('#ganttTable thead th');
//                const timeColWidth = $headerCells.eq(1).outerWidth(); // width of one time cell

//                const offsetLeftPx = ((actualStart - minDate) / intervalMs) * timeColWidth;
//                const barWidthPx = ((actualEnd - actualStart) / intervalMs) * timeColWidth;

//                // Clamp to timeline bounds for safety
//                const maxWidthPx = totalCols * timeColWidth;
//                const safeLeft = Math.max(0, Math.min(offsetLeftPx, maxWidthPx));
//                const safeWidth = Math.max(0, Math.min(barWidthPx, maxWidthPx - safeLeft));

//                actRow.append(`
//          <td class="gantt-actual" colspan="${totalCols}">
//            <div class="actual-bar ${isOngoing ? "ongoing" : ""}"
//                 style="margin-left:${safeLeft}px; width:${safeWidth}px;"></div>
//          </td>
//        `);
//            }
//        } else {
//            // Missing actuals: show empty timeline cell
//            actRow.append(`<td class="gantt-actual" colspan="${totalCols}"></td>`);
//        }

//        table.find("#ganttBody").append(actRow);




//    });

//    // Today Line
//    const today = new Date();
//    if (!isNaN(minDate.getTime()) && !isNaN(maxDate.getTime()) && totalCols > 1) {
//        const todayIndex = Math.floor((today - minDate) / intervalMs);
//        if (todayIndex >= 0 && todayIndex < totalCols) {
//            const $headerCells = $('#ganttTable thead th');
//            if ($headerCells.length > 1) {
//                const taskColWidth = $headerCells.eq(0).outerWidth();
//                const timeColWidth = $headerCells.eq(1).outerWidth();
//                const leftOffset = taskColWidth + (todayIndex * timeColWidth) + (timeColWidth / 2);
//                scrollContainer.find('.gantt-today-line').remove();
//                scrollContainer.append(
//                    `<div class="gantt-today-line" style="left:${leftOffset}px; height:${$('#ganttTable').height()}px;"></div>`
//                );
//            }
//        }
//    }

//    highlightActiveRow(wipName);
//}


// Inject CSS - Uniform Design Across Both Panels
//const style = document.createElement('style');
//style.innerHTML = `
//    /* Main Layout - Consistent across both panels */
//    #pageLayout {
//        display: flex;
//        min-height: calc(100vh - 400px);
//        background: #f5f7fa;
//        gap: 0;
//        border-radius: 12px;
//        overflow: hidden;
//        box-shadow: 0 4px 20px rgba(0,0,0,0.08);
//    }

//    /* Left Panel - Compact WIP List */
//    #leftWipPanel {
//        width: 15%;
//        min-width: 280px;
//        background: white;
//        border-right: 1px solid #dee2e6;
//        box-shadow: 4px 0 15px rgba(0,0,0,0.1);
//        overflow-y: auto;
//        padding: 16px;
//        display: flex;
//        flex-direction: column;
//    }

//    #leftWipPanel h2 {
//        font-size: 1.15rem;
//        color: #34495e;
//        margin: 0 0 12px 0;
//        text-align: left;
//        font-weight: 600;
//    }

//    /* Compact WIP Table - Uniform with Gantt */
//    #wipListTable {
//        width: 100%;
//        border-collapse: collapse;
//        font-size: 0.85rem;
//        flex: 1;
//    }

//    #wipListTable th {
//        background: #34495e;
//        color: white;
//        padding: 8px 10px;
//        font-size: 0.82rem;
//        text-align: left;
//        border: none;
//    }

//    #wipListTable td {
//        padding: 8px 10px;
//        border-bottom: 1px solid #e9ecef;
//        vertical-align: middle;
//    }

//    #wipListTable strong {
//        font-size: 0.92rem;
//        color: #2c3e50;
//    }

//    /* Small "View Gantt" button - Matches Gantt button style */
//    #wipListTable .show-gantt-btn {
//        padding: 6px 12px;
//        font-size: 0.8rem;
//        border-radius: 6px;
//        background: linear-gradient(135deg, #3498db, #2980b9);
//        color: white;
//        border: none;
//        cursor: pointer;
//        width: 100%;
//        transition: all 0.2s;
//        font-weight: 500;
//        text-transform: none;
//    }

//    #wipListTable .show-gantt-btn:hover {
//        background: linear-gradient(135deg, #2980b9, #1f618d);
//        transform: translateY(-1px);
//        box-shadow: 0 2px 8px rgba(52, 152, 219, 0.3);
//    }

//    .active-wip-row {
//        background: #e8f4fd !important;
//        font-weight: 600;
//        border-left: 4px solid #3498db;
//    }

//    /* Right Panel - Matches Left Panel Design */
//    #rightGanttPanel {
//        width: 85%;
//        padding: 16px;
//        overflow: auto;
//        position: relative;
//        background: white;
//    }

//    .blank-state {
//        display: flex;
//        align-items: center;
//        justify-content: center;
//        height: 100%;
//        font-size: 1.4rem;
//        color: #6c757d;
//        text-align: center;
//        font-style: italic;
//        background: #f8f9fa;
//        border-radius: 8px;
//        padding: 40px;
//    }

//    /* Gantt Header - Uniform with Left Panel */
//    .gantt-header {
//        background: linear-gradient(135deg, #34495e, #2c3e50);
//        color: white;
//        padding: 12px 16px;
//        font-size: 1.1rem;
//        display: flex;
//        justify-content: space-between;
//        align-items: center;
//        border-radius: 8px;
//        margin-bottom: 12px;
//        font-weight: 600;
//    }

//    .gantt-header small {
//        font-size: 0.85rem;
//        opacity: 0.8;
//    }

//    .gantt-controls {
//        display: flex;
//        align-items: center;
//        gap: 10px;
//    }

//    .gantt-controls label {
//        font-size: 0.9rem;
//        margin: 0;
//        font-weight: 500;
//    }

//    #ganttViewToggle {
//        padding: 6px 10px;
//        border-radius: 6px;
//        border: none;
//        background: rgba(255,255,255,0.15);
//        color: black;
//        font-size: 0.85rem;
//        min-width: 100px;
//    }

//    .download-pdf-btn {
//        padding: 6px 12px;
//        border-radius: 6px;
//        background: rgba(255,255,255,0.15);
//        border: none;
//        color: white;
//        cursor: pointer;
//        font-size: 0.85rem;
//        font-weight: 500;
//        transition: all 0.2s;
//    }

//    .download-pdf-btn:hover {
//        background: rgba(255,255,255,0.25);
//        transform: translateY(-1px);
//    }

//    /* Gantt Scroll Container - Matches Left Panel */
//    #ganttScrollContainer {
//        background: white;
//        border-radius: 8px;
//        box-shadow: 0 2px 12px rgba(0,0,0,0.08);
//        overflow: auto;
//        padding: 16px;
//        position: relative;
//        border: 1px solid #e9ecef;
//        overflow: visible;
//    }

//    /* Gantt Table - Compact & Uniform */
//    #ganttTable {
//        width: max-content;
//        min-width: 100%;
//        border-collapse: separate;
//        border-spacing: 0;
//        font-size: 0.85rem; /* Matches WIP table */
//    }

//    #ganttTable th {

//        color: black;
//        font-weight: 600;
//        padding: 8px 6px; /* Reduced padding */
//        border-bottom: 2px solid #2c3e50;
//        position: sticky;
//        top: 0;
//        z-index: 10;
//        min-width: 32px; /* Slightly smaller */
//        font-size: 0.8rem;
//        border: none;
//    }

//    #ganttTable td:first-child,
//    #ganttTable th:first-child {
//        position: sticky;
//        left: 0;
//        background: #f8f9fa !important;
//        min-width: 240px; /* Slightly smaller */
//        padding: 8px 12px; /* Reduced padding */
//        z-index: 9;
//        box-shadow: 4px 0 8px rgba(0,0,0,0.06);
//        border-right: 2px solid #dee2e6;
//    }

//    /* Row alternating colors - matches WIP table */
//    #ganttTable tbody tr:nth-child(even) { background-color: #f8f9fa; }
//    #ganttTable tbody tr:nth-child(odd) { background-color: white; }

//    /* Scheduled Task Cells - Compact */
//    .gantt-scheduled {
//        min-width: 32px;
//        height: 24px; /* Reduced height */
//        position: relative;
//        border: 1px solid #dee2e6;
//        font-size: 0.75rem;
//    }

// .gantt-scheduled.pending {
//    background: linear-gradient(135deg, #fff9c4, #fff3b8); /* Light Yellow */
//}

//.gantt-scheduled.running {
//    background: linear-gradient(135deg, #fce4e4, #f9b4b4); /* Soft Light Red - Active */
//}

//.gantt-scheduled.completed {
//    background: linear-gradient(135deg, #d4edda, #a3e9b3); /* Light Green */
//}

//.gantt-scheduled.pause {
//    background: linear-gradient(135deg, #ffe8cc, #ffbb8e); /* Light Amber - Warning */
//}
//    .gantt-scheduled.occupied {
//        color: #2c3e50;
//        font-weight: 700;
//        font-size: 0.75rem;
//        text-align: center;
//        position: relative;
//        padding: 2px;
//    }

//    /* Actual Progress - Compact */
//    .gantt-actual {
//        min-width: 32px;
//        height: 28px; /* Reduced height */
//        position: relative;
//        border: 1px solid #dee2e6;
//        background: #f8f9fa;
//    }

//    .actual-bar {
//        position: absolute;
//        height: 12px; /* Reduced height */
//        top: 50%;
//        transform: translateY(-50%);
//        background: linear-gradient(90deg, #27ae60, #219653);
//        border-radius: 4px;
//        border: 1px solid #27ae60;
//        box-shadow: 0 2px 6px rgba(39, 174, 96, 0.3);
//        min-width: 4px;
//    }

//    .actual-bar.ongoing {
//        opacity: 0.8;
//        animation: pulse 2s infinite;
//    }

//    @keyframes pulse {
//        0% { opacity: 0.8; }
//        50% { opacity: 0.6; }
//        100% { opacity: 0.8; }
//    }

//    /* Tooltip - Compact & Consistent */
//    .gantt-scheduled.occupied::after {
//        content: attr(data-tooltip);
//        position: absolute;
//        left: 50%;
//        transform: translateX(calc(-50% + var(--tooltip-offset-x, 0px)));
//        bottom: 100%;
//        margin-bottom: 8px;
//        background: rgba(0,0,0,0.92);
//        color: #fff;
//        padding: 10px 12px;
//        border-radius: 6px;
//        font-size: 11px;
//        line-height: 1.4;
//        white-space: normal;
//        word-wrap: break-word;
//        z-index: 9999;
//        min-width: 200px;
//        max-width: 320px;
//        width: max-content;
//        box-shadow: 0 8px 25px rgba(0,0,0,0.3);
//        opacity: 0;
//        visibility: hidden;
//        transition: opacity 0.2s ease, visibility 0.2s;
//        pointer-events: none;
//        text-align: left;
//    }

//    .gantt-scheduled.occupied::before {
//        content: '';
//        position: absolute;
//        left: 50%;
//        transform: translateX(-50%);
//        border: 8px solid transparent;
//        border-top-color: rgba(0,0,0,0.92);
//        bottom: 100%;
//        margin-bottom: 2px;
//        opacity: 0;
//        visibility: hidden;
//        transition: opacity 0.2s ease, visibility 0.2s;
//        z-index: 9999;
//    }

//    .gantt-scheduled.occupied:hover::after,
//    .gantt-scheduled.occupied:hover::before {
//        opacity: 1;
//        visibility: visible;
//    }

//    .gantt-scheduled.occupied:hover[data-flip="true"]::after {
//        top: 100%;
//        bottom: auto;
//        margin-top: 8px;
//    }

//    .gantt-scheduled.occupied:hover[data-flip="true"]::before {
//        top: 100%;
//        border-top-color: transparent;
//        border-bottom-color: rgba(0,0,0,0.92);
//        margin-top: 2px;
//    }

//    /* Task Labels - Compact */
//    .task-label {
//        font-size: 12px; /* Reduced */
//        color: #2c3e50;
//        display: flex;
//        flex-direction: column;
//        justify-content: center;
//        line-height: 1.3;
//    }

//    .task-label small {
//        font-size: 10px;
//        color: #6c757d;
//        margin-top: 2px;
//    }

//    .actual-label {
//        font-size: 10px; /* Reduced */
//        color: #6c757d;
//        font-style: italic;
//        padding-left: 12px;
//        font-weight: 500;
//    }

//    /* Error States - Consistent */
//    .gantt-actual.error {
//        background: #fff5f5;
//        border-color: #f1aeb5;
//    }

//    .actual-error-text {
//        color: #c1121f;
//        font-weight: 600;
//        font-size: 10px;
//        padding: 4px 8px;
//        display: block;
//        text-align: center;
//    }

//    /* Hover effects for better UX */
//    #ganttTable tr:hover {
//        background: #f0f8ff !important;
//    }

//    .gantt-scheduled.occupied:hover {
//        transform: scale(1.05);
//        z-index: 20;
//        box-shadow: 0 2px 8px rgba(0,0,0,0.15);
//    }
//`;
//document.head.appendChild(style);

//// Global state
//let ganttView = 'hour';
//let currentGrouped = null;
//let currentItemId = null;
//let currentItemName = null;
//let activeWipName = null;
//let pdfReady = false;
//let autoRefreshInterval = null;

//$(document).ready(function () {
//    $("#itemSearchButton").on('click', function () {
//        const locationId = $("#LocationIdSearch").val() || "";
//        const businessUnitId = $("#BusinessUnitIdSearch").val() || "";
//        const sectionId = $("#SectionIdSearch").val() || "";
//        loadItemTable(locationId, businessUnitId, sectionId);
//    });

//    $("#BusinessUnitIdSearch, #LocationIdSearch, #SectionIdSearch").on('keydown', function (e) {
//        if (e.key === 'Enter') {
//            e.preventDefault();
//            $("#itemSearchButton").trigger('click');
//        }
//    });
//});

//// Wait for jsPDF
//(function waitForPDF() {
//    if (window.jspdf && window.jspdf.jsPDF && window.jspdf.jsPDF.autoTable) {
//        pdfReady = true;
//    } else {
//        setTimeout(waitForPDF, 500);
//    }
//})();

//function generatePDF(wipName) {
//    const { jsPDF } = window.jspdf;
//    const doc = new jsPDF('l', 'mm', 'a4');
//    wipName = wipName || 'Gantt';
//    doc.setFontSize(18);
//    doc.text(`Gantt Chart - WIP: ${wipName}`, 14, 20);
//    doc.setFontSize(12);
//    doc.text(`Generated on: ${new Date().toLocaleString()}`, 14, 30);
//    doc.autoTable({
//        html: '#ganttTable',
//        startY: 40,
//        theme: 'grid',
//        styles: { fontSize: 8, cellPadding: 3 },
//        headStyles: { fillColor: [52, 71, 94], textColor: 255, fontStyle: 'bold' },
//        alternateRowStyles: { fillColor: [248, 249, 250] },
//        columnStyles: { 0: { cellWidth: 60, fontStyle: 'bold' } },
//        margin: { top: 40, left: 10, right: 10 },
//        didDrawPage: (data) => {
//            doc.setFontSize(10);
//            doc.text(`Page ${data.pageNumber}`, 280, 200);
//        }
//    });
//    doc.save(`Gantt_${wipName.replace(/[^a-z0-9]/gi, '_')}_${new Date().toISOString().slice(0, 10)}.pdf`);
//}

//function loadItemTable(locationId, businessUnitId, sectionId) {
//    if ($.fn.DataTable.isDataTable("#itemTable")) {
//        $("#itemTable").DataTable().destroy();
//    }
//    $("#itemTable").DataTable({
//        processing: true,
//        serverSide: true,
//        ajax: {
//            url: "/item/getall",
//            type: "POST",
//            data: { LocationId: locationId, BusinessUnitId: businessUnitId, SectionId: sectionId }
//        },
//        columns: [
//            { data: "itemName" },
//            { data: "section" },
//            { data: "itemConfigurationType" },
//            { data: "machineType" },
//            { data: "itemName" },
//            { data: "itemCode" },
//            { data: "sct" },
//            { data: "cavity" },
//            { data: "weight" },
//            {
//                data: null,
//                orderable: false,
//                render: function (row) {
//                    return `<button class="btn btn-primary btn-sm wip-btn" data-itemid="${row.id}" data-itemname="${row.itemName}">Show WIP</button>`;
//                }
//            }
//        ]
//    });
//}

//$(document).on('click', '.wip-btn', function () {
//    const itemId = $(this).data('itemid');
//    const itemName = $(this).data('itemname');
//    showWipListInLeftPanel(itemId, itemName);
//});

//function showWipListInLeftPanel(itemId, itemName) {
//    $("#pageLayout").html(`
//        <div id="leftWipPanel">
//            <h2>WIPs for Item: ${itemName}</h2>
//            <table id="wipListTable">
//                <thead><tr><th>WIP Name</th><th style="width:90px;">Action</th></tr></thead>
//                <tbody></tbody>
//            </table>
//        </div>
//        <div id="rightGanttPanel">
//            <div class="blank-state">Select a WIP and click "View Gantt" to see the chart</div>
//        </div>
//    `);

//    currentItemId = itemId;
//    currentItemName = itemName;
//    activeWipName = null;
//    if (autoRefreshInterval) clearInterval(autoRefreshInterval);
//    loadWipTasks(itemId);
//}

//function loadWipTasks(itemId) {
//    $.post("/ToolRoomDashboard/getWIPGanttByItemId", { itemId }, function (tasks) {
//        if (!tasks || tasks.length === 0) {
//            $("#wipListTable tbody").html("<tr><td colspan='2'>No WIPs found</td></tr>");
//            return;
//        }
//        currentGrouped = {};
//        tasks.forEach(t => (currentGrouped[t.name] ||= []).push(t));
//        const tbody = $("#wipListTable tbody").empty();
//        Object.keys(currentGrouped).sort().forEach(wipName => {
//            tbody.append(`
//                <tr data-wip="${wipName}">
//                    <td><strong>${wipName}</strong></td>
//                    <td><button class="show-gantt-btn" data-wip="${wipName}">View Gantt</button></td>
//                </tr>
//            `);
//        });
//        if (activeWipName && currentGrouped[activeWipName]) {
//            renderGanttInRightPanel(activeWipName, currentGrouped[activeWipName]);
//        }
//    });
//}

//$(document).on('click', '.show-gantt-btn', function () {
//    const wipName = $(this).data('wip');
//    activeWipName = wipName;
//    renderGanttInRightPanel(wipName, currentGrouped[wipName]);
//    highlightActiveRow(wipName);

//    if (autoRefreshInterval) clearInterval(autoRefreshInterval);
//    autoRefreshInterval = setInterval(() => {
//        if (currentItemId && activeWipName) {
//            loadWipTasks(currentItemId);
//        }
//    }, 30000);
//});

//$(document).on('change', '#ganttViewToggle', function () {
//    ganttView = $(this).val();
//    if (activeWipName && currentGrouped && currentGrouped[activeWipName]) {
//        renderGanttInRightPanel(activeWipName, currentGrouped[activeWipName]);
//    }
//});

//$(document).on('click', '.download-pdf-btn', function () {
//    if (!pdfReady) {
//        alert('PDF library is still loading. Please wait a few seconds and try again.');
//        return;
//    }
//    generatePDF(activeWipName);
//});

//function highlightActiveRow(wipName) {
//    $("#wipListTable tr").removeClass("active-wip-row");
//    $(`#wipListTable tr[data-wip="${wipName}"]`).addClass("active-wip-row");
//}

//function updateTooltipPositions() {
//    document.querySelectorAll('.gantt-scheduled.occupied').forEach(cell => {
//        const rect = cell.getBoundingClientRect();
//        if (rect.top < 160) {
//            cell.setAttribute('data-flip', 'true');
//        } else {
//            cell.removeAttribute('data-flip');
//        }
//        let offsetX = 0;
//        const half = 160;
//        if (rect.left + half > window.innerWidth) {
//            offsetX = window.innerWidth - (rect.left + half) - 20;
//        } else if (rect.right - half < 0) {
//            offsetX = -(rect.right - half) + 20;
//        }
//        if (offsetX !== 0) {
//            cell.style.setProperty('--tooltip-offset-x', `${offsetX}px`);
//        } else {
//            cell.style.removeProperty('--tooltip-offset-x');
//        }
//    });
//}

//function renderGanttInRightPanel(wipName, tasks) {
//    const rightPanel = $("#rightGanttPanel").empty();
//    if (!tasks || tasks.length === 0) {
//        rightPanel.html('<div class="blank-state">No tasks found for this WIP</div>');
//        return;
//    }

//    rightPanel.append(`
//        <div class="gantt-header">
//            <span>Gantt Chart — ${wipName} <small>(Auto-refresh every 30s)</small></span>
//            <div class="gantt-controls">
//                <label>View:</label>
//                <select id="ganttViewToggle">
//                    <option value="hour">Hour View</option>
//                    <option value="day">Day View</option>
//                </select>
//                <button class="download-pdf-btn">Download PDF</button>
//            </div>
//        </div>
//        <div id="ganttScrollContainer"><div id="ganttContainer"></div></div>
//    `);
//    $("#ganttViewToggle").val(ganttView);

//    const container = $("#ganttContainer");
//    const table = $(`<table id="ganttTable"><thead id="ganttHeader"></thead><tbody id="ganttBody"></tbody></table>`);
//    container.append(table);

//    const allDates = tasks.flatMap(t => [
//        new Date(t.startTime), new Date(t.endTime),
//        t.actualStartTime ? new Date(t.actualStartTime) : null,
//        t.actualEndTime ? new Date(t.actualEndTime) : null
//    ]).filter(Boolean);

//    const minDate = new Date(Math.min(...allDates));
//    const maxDate = new Date(Math.max(...allDates));
//    const totalDuration = maxDate - minDate || 1;
//    const intervalMs = ganttView === 'hour' ? 30 * 60 * 1000 : 24 * 60 * 60 * 1000;
//    const totalCols = Math.ceil(totalDuration / intervalMs) + 1;

//    // Header
//    const headerRow = $("<tr></tr>").append("<th rowspan='2'>Task Details</th>");
//    for (let i = 0; i < totalCols; i++) {
//        const d = new Date(minDate.getTime() + i * intervalMs);
//        const label = ganttView === 'hour'
//            ? `${d.getDate().toString().padStart(2, '0')}/${(d.getMonth() + 1).toString().padStart(2, '0')} ${d.getHours().toString().padStart(2, '0')}:${d.getMinutes().toString().padStart(2, '0')}`
//            : `${d.getDate().toString().padStart(2, '0')}/${(d.getMonth() + 1).toString().padStart(2, '0')}/${d.getFullYear()}`;
//        headerRow.append(`<th>${label}</th>`);
//    }
//    $("#ganttHeader").append(headerRow);
//    $("#ganttHeader").append(`<tr><th>Actual Progress</th>${'<th></th>'.repeat(totalCols)}</tr>`);

//    tasks.sort((a, b) => new Date(a.startTime) - new Date(b.startTime));

//    tasks.forEach(t => {
//        if (!t.status || t.status === "Null") return;

//        const scheduledStart = new Date(t.startTime);
//        const scheduledEnd = new Date(t.endTime);
//        const sStartIdx = Math.max(0, Math.floor((scheduledStart - minDate) / intervalMs));
//        const sEndIdx = Math.min(totalCols - 1, Math.floor((scheduledEnd - minDate) / intervalMs));

//        const taskLabel = `${t.taskName || 'Task'}${t.machineName ? ' — ' + t.machineName : ''}<br><small>Status: ${t.status}${t.operatorName ? ' (' + t.operatorName + ')' : ''}</small>`;
//        const tooltip = `Task: ${t.taskName || ''}\nMachine: ${t.machineName || ''}\nOperator: ${t.operatorName || ''}\nScheduled: ${scheduledStart.toLocaleString()} → ${scheduledEnd.toLocaleString()}\nActual: ${t.actualStartTime ? new Date(t.actualStartTime).toLocaleString() : 'N/A'} → ${t.actualEndTime ? new Date(t.actualEndTime).toLocaleString() : 'N/A'}\nStatus: ${t.status}\nTotal Time: ${t.totalTime || 0} hrs`;

//        let statusClass = t.status.toLowerCase() === "running" ? "running" :
//            t.status.toLowerCase() === "completed" ? "completed" :
//                t.status.toLowerCase() === "pause" ? "pause" : "pending";

//        const schRow = $("<tr></tr>").append(`<td class="task-label">${taskLabel}</td>`);
//        let i = 0;
//        while (i < totalCols) {
//            if (i < sStartIdx || i > sEndIdx) {
//                schRow.append("<td class='gantt-scheduled'></td>");
//                i++;
//            } else {
//                const colspan = sEndIdx - sStartIdx + 1;
//                schRow.append(`<td class="gantt-scheduled occupied ${statusClass}" colspan="${colspan}" data-tooltip="${tooltip}">${t.status.toUpperCase()}</td>`);
//                i = sEndIdx + 1;
//            }
//        }
//        $("#ganttBody").append(schRow);

//        // Actual Progress (Percentage-based)
//        const actRow = $("<tr></tr>").append(`<td class="actual-label">Actual</td>`);
//        let actualStart = t.actualStartTime ? new Date(t.actualStartTime) : null;
//        let actualEnd = t.actualEndTime ? new Date(t.actualEndTime) : (["running", "pause"].includes(t.status?.toLowerCase()) ? new Date() : null);
//        let isOngoing = !t.actualEndTime && actualStart;

//        if (actualStart && actualEnd && actualEnd < actualStart) {
//            actRow.append(`<td class="gantt-actual error" colspan="${totalCols}"><span class="actual-error-text">Actual end before start — verify data</span></td>`);
//        } else if (actualStart && actualEnd && totalDuration > 0) {
//            const startPct = Math.max(0, Math.min(100, ((actualStart - minDate) / totalDuration) * 100));
//            const widthPct = Math.max(1, Math.min(100, ((actualEnd - actualStart) / totalDuration) * 100));
//            actRow.append(`
//                <td class="gantt-actual" colspan="${totalCols}" style="position: relative;">
//                    <div class="actual-bar ${isOngoing ? "ongoing" : ""}" style="left:${startPct}%; width:${widthPct}%;"></div>
//                </td>
//            `);
//        } else {
//            actRow.append(`<td class="gantt-actual" colspan="${totalCols}"></td>`);
//        }
//        $("#ganttBody").append(actRow);
//    });





//    highlightActiveRow(wipName);
//    setTimeout(updateTooltipPositions, 150);
//    $('#ganttScrollContainer').off('scroll', updateTooltipPositions).on('scroll', updateTooltipPositions);
//    $(window).off('resize', updateTooltipPositions).on('resize', updateTooltipPositions);
//}



// Inject CSS - Uniform, Professional Design
const style = document.createElement('style');
style.innerHTML = `
    /* Main Layout */
    #pageLayout {
        display: flex;
        min-height: calc(100vh - 400px);
        background: #f5f7fa;
        gap: 0;
        border-radius: 12px;
        overflow: hidden;
        box-shadow: 0 4px 20px rgba(0,0,0,0.08);
    }

    /* Left Panel - Compact WIP List */
    #leftWipPanel {
        width: 15%;
        min-width: 280px;
        background: white;
        border-right: 1px solid #dee2e6;
        box-shadow: 4px 0 15px rgba(0,0,0,0.1);
        overflow-y: auto;
        padding: 16px;
        display: flex;
        flex-direction: column;
    }

    #leftWipPanel h2 {
        font-size: 1.15rem;
        color: #34495e;
        margin: 0 0 12px 0;
        text-align: left;
        font-weight: 600;
    }

    #wipListTable {
        width: 100%;
        border-collapse: collapse;
        font-size: 0.85rem;
        flex: 1;
    }

    #wipListTable th {
        background: #34495e;
        color: white;
        padding: 8px 10px;
        font-size: 0.82rem;
        text-align: left;
        border: none;
    }

    #wipListTable td {
        padding: 8px 10px;
        border-bottom: 1px solid #e9ecef;
        vertical-align: middle;
    }

    #wipListTable strong {
        font-size: 0.92rem;
        color: #2c3e50;
    }

    #wipListTable .show-gantt-btn {
        padding: 6px 12px;
        font-size: 0.8rem;
        border-radius: 6px;
        background: linear-gradient(135deg, #3498db, #2980b9);
        color: white;
        border: none;
        cursor: pointer;
        width: 100%;
        transition: all 0.2s;
        font-weight: 500;
    }

    #wipListTable .show-gantt-btn:hover {
        background: linear-gradient(135deg, #2980b9, #1f618d);
        transform: translateY(-1px);
        box-shadow: 0 2px 8px rgba(52, 152, 219, 0.3);
    }

    .active-wip-row {
        background: #e8f4fd !important;
        font-weight: 600;
        border-left: 4px solid #3498db;
    }

    /* Right Panel */
    #rightGanttPanel {
        width: 85%;
        padding: 16px;
        overflow: auto;
        position: relative;
        background: white;
    }

    .blank-state {
        display: flex;
        align-items: center;
        justify-content: center;
        height: 100%;
        font-size: 1.4rem;
        color: #6c757d;
        text-align: center;
        font-style: italic;
        background: #f8f9fa;
        border-radius: 8px;
        padding: 40px;
    }

    /* Gantt Header - Clean Navy Blue */
    .gantt-header {
        background: #1e40af;
        color: white;
        padding: 12px 16px;
        font-size: 1.1rem;
        display: flex;
        justify-content: space-between;
        align-items: center;
        border-radius: 8px;
        margin-bottom: 12px;
        font-weight: 600;
    }

    .gantt-controls {
        display: flex;
        align-items: center;
        gap: 10px;
    }

    .gantt-controls label {
        font-size: 0.9rem;
        margin: 0;
        font-weight: 500;
    }

    #ganttViewToggle {
        padding: 6px 10px;
        border-radius: 6px;
        border: none;
        background: rgba(255,255,255,0.15);
        color: white;
        font-size: 0.85rem;
        min-width: 100px;
    }

    .download-pdf-btn {
        padding: 6px 12px;
        border-radius: 6px;
        background: rgba(255,255,255,0.15);
        border: none;
        color: white;
        cursor: pointer;
        font-size: 0.85rem;
        font-weight: 500;
        transition: all 0.2s;
    }

    .download-pdf-btn:hover {
        background: rgba(255,255,255,0.25);
        transform: translateY(-1px);
    }

    #ganttScrollContainer {
        background: white;
        border-radius: 8px;
        box-shadow: 0 2px 12px rgba(0,0,0,0.08);
        overflow: auto;
        padding: 16px;
        position: relative;
        border: 1px solid #e9ecef;
        overflow: visible;
    }

    #ganttTable {
        width: max-content;
        min-width: 100%;
        border-collapse: separate;
        border-spacing: 0;
        font-size: 0.85rem;
    }

    /* Gantt Table Header - Compact Hour View with Two Lines */
    #ganttTable th {
        color: black !important;
        font-weight: 600;
        padding: 6px 2px;
        border-bottom: 2px solid #2c3e50;
        position: sticky;
        top: 0;
        z-index: 10;
        min-width: 28px;
        width: 28px;
        font-size: 0.75rem;
        text-align: center;
        line-height: 1.2;
        white-space: normal;
        word-wrap: break-word;
        height: 50px;
        vertical-align: middle;
        border: none;
    }

    #ganttTable td:first-child,
    #ganttTable th:first-child {
        position: sticky;
        left: 0;
        background: #f8f9fa !important;
        min-width: 240px;
        padding: 8px 12px;
        z-index: 9;
        box-shadow: 4px 0 8px rgba(0,0,0,0.06);
        border-right: 2px solid #dee2e6;
    }

    #ganttTable tbody tr:nth-child(even) { background-color: #f8f9fa; }
    #ganttTable tbody tr:nth-child(odd) { background-color: white; }

    /* Compact Cells for Hour View */
    .gantt-scheduled {
        min-width: 28px;
        width: 28px;
        height: 24px;
        position: relative;
        border: 1px solid #dee2e6;
        font-size: 0.7rem;
        padding: 0;
    }

    .gantt-scheduled.pending {
        background: linear-gradient(135deg, #fff9c4, #fff3b8);
    }
    .gantt-scheduled.running {
        background: linear-gradient(135deg, #fce4e4, #f9b4b4);
    }
    .gantt-scheduled.completed {
        background: linear-gradient(135deg, #d4edda, #a3e9b3);
    }
    .gantt-scheduled.pause {
        background: linear-gradient(135deg, #ffe8cc, #ffbb8e);
    }

    .gantt-scheduled.occupied {
        color: #2c3e50;
        font-weight: 700;
        font-size: 0.7rem;
        text-align: center;
        position: relative;
        padding: 2px;
    }

    .gantt-actual {
        min-width: 28px;
        width: 28px;
        height: 28px;
        position: relative;
        border: 1px solid #dee2e6;
        background: #f8f9fa;
    }

    .actual-bar {
        position: absolute;
        height: 12px;
        top: 50%;
        transform: translateY(-50%);
        background: linear-gradient(90deg, #27ae60, #219653);
        border-radius: 4px;
        border: 1px solid #27ae60;
        box-shadow: 0 2px 6px rgba(39, 174, 96, 0.3);
        min-width: 4px;
    }

    .actual-bar.ongoing {
        opacity: 0.8;
        animation: pulse 2s infinite;
    }

    @keyframes pulse {
        0% { opacity: 0.8; }
        50% { opacity: 0.6; }
        100% { opacity: 0.8; }
    }

    /* Tooltip - Always Above Header */
    .gantt-scheduled.occupied::after {
        content: attr(data-tooltip);
        position: absolute;
        left: 50%;
        transform: translateX(calc(-50% + var(--tooltip-offset-x, 0px)));
        bottom: 100%;
        margin-bottom: 8px;
        background: rgba(0,0,0,0.92);
        color: #fff;
        padding: 10px 12px;
        border-radius: 6px;
        font-size: 11px;
        line-height: 1.4;
        white-space: normal;
        word-wrap: break-word;
        z-index: 9999;
        min-width: 200px;
        max-width: 320px;
        width: max-content;
        box-shadow: 0 8px 25px rgba(0,0,0,0.3);
        opacity: 0;
        visibility: hidden;
        transition: opacity 0.2s ease, visibility 0.2s;
        pointer-events: none;
        text-align: left;
    }

    .gantt-scheduled.occupied::before {
        content: '';
        position: absolute;
        left: 50%;
        transform: translateX(-50%);
        border: 8px solid transparent;
        border-top-color: rgba(0,0,0,0.92);
        bottom: 100%;
        margin-bottom: 2px;
        opacity: 0;
        visibility: hidden;
        transition: opacity 0.2s ease, visibility 0.2s;
        z-index: 9999;
    }

    .gantt-scheduled.occupied:hover::after,
    .gantt-scheduled.occupied:hover::before {
        opacity: 1;
        visibility: visible;
    }

    .gantt-scheduled.occupied:hover[data-flip="true"]::after {
        top: 100%;
        bottom: auto;
        margin-top: 8px;
    }

    .gantt-scheduled.occupied:hover[data-flip="true"]::before {
        top: 100%;
        border-top-color: transparent;
        border-bottom-color: rgba(0,0,0,0.92);
        margin-top: 2px;
    }

    .task-label {
        font-size: 12px;
        color: #2c3e50;
        display: flex;
        flex-direction: column;
        justify-content: center;
        line-height: 1.3;
    }

    .task-label small {
        font-size: 10px;
        color: #6c757d;
        margin-top: 2px;
    }

    .actual-label {
        font-size: 10px;
        color: #6c757d;
        font-style: italic;
        padding-left: 12px;
        font-weight: 500;
    }

    .gantt-actual.error {
        background: #fff5f5;
        border-color: #f1aeb5;
    }

    .actual-error-text {
        color: #c1121f;
        font-weight: 600;
        font-size: 10px;
        padding: 4px 8px;
        display: block;
        text-align: center;
    }

    #ganttTable tr:hover {
        background: #f0f8ff !important;
    }

    .gantt-scheduled.occupied:hover {
        transform: scale(1.05);
        z-index: 20;
        box-shadow: 0 2px 8px rgba(0,0,0,0.15);
    }
`;
document.head.appendChild(style);

// Global state
let ganttView = 'hour';
let currentGrouped = null;
let currentItemId = null;
let currentItemName = null;
let activeWipName = null;
let pdfReady = false;
let autoRefreshInterval = null;

$(document).ready(function () {
    $("#itemSearchButton").on('click', function () {
        const locationId = $("#LocationIdSearch").val() || "";
        const businessUnitId = $("#BusinessUnitIdSearch").val() || "";
        const sectionId = $("#SectionIdSearch").val() || "";
        loadItemTable(locationId, businessUnitId, sectionId);
    });

    $("#BusinessUnitIdSearch, #LocationIdSearch, #SectionIdSearch").on('keydown', function (e) {
        if (e.key === 'Enter') {
            e.preventDefault();
            $("#itemSearchButton").trigger('click');
        }
    });
});

// Wait for jsPDF
(function waitForPDF() {
    if (window.jspdf && window.jspdf.jsPDF && window.jspdf.jsPDF.autoTable) {
        pdfReady = true;
    } else {
        setTimeout(waitForPDF, 500);
    }
})();

function generatePDF(wipName) {
    const { jsPDF } = window.jspdf;
    const doc = new jsPDF('l', 'mm', 'a4');
    wipName = wipName || 'Gantt';
    doc.setFontSize(18);
    doc.text(`Gantt Chart - WIP: ${wipName}`, 14, 20);
    doc.setFontSize(12);
    doc.text(`Generated on: ${new Date().toLocaleString()}`, 14, 30);
    doc.autoTable({
        html: '#ganttTable',
        startY: 40,
        theme: 'grid',
        styles: { fontSize: 8, cellPadding: 3 },
        headStyles: { fillColor: [52, 71, 94], textColor: 255, fontStyle: 'bold' },
        alternateRowStyles: { fillColor: [248, 249, 250] },
        columnStyles: { 0: { cellWidth: 60, fontStyle: 'bold' } },
        margin: { top: 40, left: 10, right: 10 },
        didDrawPage: (data) => {
            doc.setFontSize(10);
            doc.text(`Page ${data.pageNumber}`, 280, 200);
        }
    });
    doc.save(`Gantt_${wipName.replace(/[^a-z0-9]/gi, '_')}_${new Date().toISOString().slice(0, 10)}.pdf`);
}

function loadItemTable(locationId, businessUnitId, sectionId) {
    if ($.fn.DataTable.isDataTable("#itemTable")) {
        $("#itemTable").DataTable().destroy();
    }
    $("#itemTable").DataTable({
        processing: true,
        serverSide: true,
        ajax: {
            url: "/item/getall",
            type: "POST",
            data: { LocationId: locationId, BusinessUnitId: businessUnitId, SectionId: sectionId }
        },
        columns: [
            { data: "itemName" },
            { data: "section" },
            { data: "itemConfigurationType" },
            { data: "machineType" },
            { data: "itemName" },
            { data: "itemCode" },
            { data: "sct" },
            { data: "cavity" },
            { data: "weight" },
            {
                data: null,
                orderable: false,
                render: function (row) {
                    return `<button class="btn btn-primary btn-sm wip-btn" data-itemid="${row.id}" data-itemname="${row.itemName}">Show WIP</button>`;
                }
            }
        ]
    });
}

$(document).on('click', '.wip-btn', function () {
    const itemId = $(this).data('itemid');
    const itemName = $(this).data('itemname');
    showWipListInLeftPanel(itemId, itemName);
});

function showWipListInLeftPanel(itemId, itemName) {
    $("#pageLayout").html(`
        <div id="leftWipPanel">
            <h2>WIPs for Item: ${itemName}</h2>
            <table id="wipListTable">
                <thead><tr><th>WIP Name</th><th style="width:90px;">Action</th></tr></thead>
                <tbody></tbody>
            </table>
        </div>
        <div id="rightGanttPanel">
            <div class="blank-state">Select a WIP and click "View Gantt" to see the chart</div>
        </div>
    `);

    currentItemId = itemId;
    currentItemName = itemName;
    activeWipName = null;
    if (autoRefreshInterval) clearInterval(autoRefreshInterval);
    loadWipTasks(itemId);
}

function loadWipTasks(itemId) {
    $.post("/ToolRoomDashboard/getWIPGanttByItemId", { itemId }, function (tasks) {
        if (!tasks || tasks.length === 0) {
            $("#wipListTable tbody").html("<tr><td colspan='2'>No WIPs found</td></tr>");
            return;
        }
        currentGrouped = {};
        tasks.forEach(t => (currentGrouped[t.name] ||= []).push(t));
        const tbody = $("#wipListTable tbody").empty();
        Object.keys(currentGrouped).sort().forEach(wipName => {
            tbody.append(`
                <tr data-wip="${wipName}">
                    <td><strong>${wipName}</strong></td>
                    <td><button class="show-gantt-btn" data-wip="${wipName}">View Gantt</button></td>
                </tr>
            `);
        });
        if (activeWipName && currentGrouped[activeWipName]) {
            renderGanttInRightPanel(activeWipName, currentGrouped[activeWipName]);
        }
    });
}

$(document).on('click', '.show-gantt-btn', function () {
    const wipName = $(this).data('wip');
    activeWipName = wipName;
    renderGanttInRightPanel(wipName, currentGrouped[wipName]);
    highlightActiveRow(wipName);

    if (autoRefreshInterval) clearInterval(autoRefreshInterval);
    autoRefreshInterval = setInterval(() => {
        if (currentItemId && activeWipName) {
            loadWipTasks(currentItemId);
        }
    }, 30000);
});

$(document).on('change', '#ganttViewToggle', function () {
    ganttView = $(this).val();
    if (activeWipName && currentGrouped && currentGrouped[activeWipName]) {
        renderGanttInRightPanel(activeWipName, currentGrouped[activeWipName]);
    }
});

$(document).on('click', '.download-pdf-btn', function () {
    if (!pdfReady) {
        alert('PDF library is still loading. Please wait a few seconds and try again.');
        return;
    }
    generatePDF(activeWipName);
});

function highlightActiveRow(wipName) {
    $("#wipListTable tr").removeClass("active-wip-row");
    $(`#wipListTable tr[data-wip="${wipName}"]`).addClass("active-wip-row");
}

function updateTooltipPositions() {
    document.querySelectorAll('.gantt-scheduled.occupied').forEach(cell => {
        const rect = cell.getBoundingClientRect();
        if (rect.top < 160) {
            cell.setAttribute('data-flip', 'true');
        } else {
            cell.removeAttribute('data-flip');
        }
        let offsetX = 0;
        const half = 160;
        if (rect.left + half > window.innerWidth) {
            offsetX = window.innerWidth - (rect.left + half) - 20;
        } else if (rect.right - half < 0) {
            offsetX = -(rect.right - half) + 20;
        }
        if (offsetX !== 0) {
            cell.style.setProperty('--tooltip-offset-x', `${offsetX}px`);
        } else {
            cell.style.removeProperty('--tooltip-offset-x');
        }
    });
}

function renderGanttInRightPanel(wipName, tasks) {
    const rightPanel = $("#rightGanttPanel").empty();
    if (!tasks || tasks.length === 0) {
        rightPanel.html('<div class="blank-state">No tasks found for this WIP</div>');
        return;
    }

    rightPanel.append(`
        <div class="gantt-header">
            <span>Gantt Chart — ${wipName}</span>
            <div class="gantt-controls">
                <label>View:</label>
                <select id="ganttViewToggle">
                    <option value="hour">Hour View</option>
                    <option value="day">Day View</option>
                </select>
                <button class="download-pdf-btn">Download PDF</button>
            </div>
        </div>
        <div id="ganttScrollContainer"><div id="ganttContainer"></div></div>
    `);

    $("#ganttViewToggle").val(ganttView);

    const container = $("#ganttContainer");
    const table = $(`<table id="ganttTable"><thead id="ganttHeader"></thead><tbody id="ganttBody"></tbody></table>`);
    container.append(table);

    const allDates = tasks.flatMap(t => [
        new Date(t.startTime), new Date(t.endTime),
        t.actualStartTime ? new Date(t.actualStartTime) : null,
        t.actualEndTime ? new Date(t.actualEndTime) : null
    ]).filter(Boolean);

    const minDate = new Date(Math.min(...allDates));
    const maxDate = new Date(Math.max(...allDates));
    const totalDuration = maxDate - minDate || 1;
    const intervalMs = ganttView === 'hour' ? 30 * 60 * 1000 : 24 * 60 * 60 * 1000;
    const totalCols = Math.ceil(totalDuration / intervalMs) + 1;

    // Header with Two-Line Format in Hour View
    const headerRow = $("<tr></tr>").append("<th rowspan='2'>Task Details</th>");
    for (let i = 0; i < totalCols; i++) {
        const d = new Date(minDate.getTime() + i * intervalMs);
        let label;

        if (ganttView === 'hour') {
            const day = d.getDate().toString().padStart(2, '0');
            const month = (d.getMonth() + 1).toString().padStart(2, '0');
            const hour = d.getHours().toString().padStart(2, '0');
            const minute = d.getMinutes().toString().padStart(2, '0');
            label = `${day}/${month}<br>${hour}:${minute}`;
        } else {
            label = `${d.getDate().toString().padStart(2,'0')}/${(d.getMonth()+1).toString().padStart(2,'0')}/${d.getFullYear()}`;
        }

        headerRow.append(`<th>${label}</th>`);
    }
    $("#ganttHeader").append(headerRow);
    $("#ganttHeader").append(`<tr><th>Actual Progress</th>${'<th></th>'.repeat(totalCols)}</tr>`);

    tasks.sort((a, b) => new Date(a.startTime) - new Date(b.startTime));

    tasks.forEach(t => {
        if (!t.status || t.status === "Null") return;

        const scheduledStart = new Date(t.startTime);
        const scheduledEnd = new Date(t.endTime);
        const sStartIdx = Math.max(0, Math.floor((scheduledStart - minDate) / intervalMs));
        const sEndIdx = Math.min(totalCols - 1, Math.floor((scheduledEnd - minDate) / intervalMs));

        const taskLabel = `${t.taskName || 'Task'}${t.machineName ? ' — ' + t.machineName : ''}<br><small>Status: ${t.status}${t.operatorName ? ' (' + t.operatorName + ')' : ''}</small>`;
        const tooltip = `Task: ${t.taskName || ''}\nMachine: ${t.machineName || ''}\nOperator: ${t.operatorName || ''}\nScheduled: ${scheduledStart.toLocaleString()} → ${scheduledEnd.toLocaleString()}\nActual: ${t.actualStartTime ? new Date(t.actualStartTime).toLocaleString() : 'N/A'} → ${t.actualEndTime ? new Date(t.actualEndTime).toLocaleString() : 'N/A'}\nStatus: ${t.status}\nTotal Time: ${t.totalTime || 0} hrs`;

        let statusClass = t.status.toLowerCase() === "running" ? "running" :
            t.status.toLowerCase() === "completed" ? "completed" :
            t.status.toLowerCase() === "pause" ? "pause" : "pending";

        const schRow = $("<tr></tr>").append(`<td class="task-label">${taskLabel}</td>`);
        let i = 0;
        while (i < totalCols) {
            if (i < sStartIdx || i > sEndIdx) {
                schRow.append("<td class='gantt-scheduled'></td>");
                i++;
            } else {
                const colspan = sEndIdx - sStartIdx + 1;
                schRow.append(`<td class="gantt-scheduled occupied ${statusClass}" colspan="${colspan}" data-tooltip="${tooltip}">${t.status.toUpperCase()}</td>`);
                i = sEndIdx + 1;
            }
        }
        $("#ganttBody").append(schRow);

        // Actual Progress Row
        const actRow = $("<tr></tr>").append(`<td class="actual-label">Actual</td>`);
        let actualStart = t.actualStartTime ? new Date(t.actualStartTime) : null;
        let actualEnd = t.actualEndTime ? new Date(t.actualEndTime) : (["running","pause"].includes(t.status?.toLowerCase()) ? new Date() : null);
        let isOngoing = !t.actualEndTime && actualStart;

        if (actualStart && actualEnd && actualEnd < actualStart) {
            actRow.append(`<td class="gantt-actual error" colspan="${totalCols}"><span class="actual-error-text">Actual end before start — verify data</span></td>`);
        } else if (actualStart && actualEnd && totalDuration > 0) {
            const startPct = Math.max(0, Math.min(100, ((actualStart - minDate) / totalDuration) * 100));
            const widthPct = Math.max(1, Math.min(100, ((actualEnd - actualStart) / totalDuration) * 100));
            actRow.append(`
                <td class="gantt-actual" colspan="${totalCols}" style="position: relative;">
                    <div class="actual-bar ${isOngoing ? "ongoing" : ""}" style="left:${startPct}%; width:${widthPct}%;"></div>
                </td>
            `);
        } else {
            actRow.append(`<td class="gantt-actual" colspan="${totalCols}"></td>`);
        }
        $("#ganttBody").append(actRow);
    });

    highlightActiveRow(wipName);
    setTimeout(updateTooltipPositions, 150);
    $('#ganttScrollContainer').off('scroll', updateTooltipPositions).on('scroll', updateTooltipPositions);
    $(window).off('resize', updateTooltipPositions).on('resize', updateTooltipPositions);
}
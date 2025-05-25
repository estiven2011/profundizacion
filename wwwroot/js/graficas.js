window.dibujarGraficoReservas = (activas, canceladas, expiradas) => {
    const ctx = document.getElementById('graficoReservas').getContext('2d');

    if (window.miGrafico) {
        window.miGrafico.destroy();
    }

    window.miGrafico = new Chart(ctx, {
        type: 'bar',
        data: {
            labels: ['Activas', 'Canceladas', 'Expiradas'],
            datasets: [{
                label: 'Reservas de hoy',
                data: [activas, canceladas, expiradas],
                backgroundColor: [
                    'rgba(25, 135, 84, 0.7)',
                    'rgba(220, 53, 69, 0.7)',
                    'rgba(108, 117, 125, 0.7)'
                ],
                borderColor: [
                    'rgba(25, 135, 84, 1)',
                    'rgba(220, 53, 69, 1)',
                    'rgba(108, 117, 125, 1)'
                ],
                borderWidth: 1
            }]
        },
        options: {
            responsive: true,
            scales: {
                y: {
                    beginAtZero: true,
                    ticks: {
                        stepSize: 1,
                        precision: 0,
                        callback: function (value) {
                            return Number.isInteger(value) ? value : null;
                        }
                    },
                    suggestedMax: 10  // <--- clave: muestra hasta 15 a menos que haya más
                }
            }


        }
    });
};

window.dibujarGraficoCanchas = (labels, datos) => {
    const ctx = document.getElementById('graficoCanchas').getContext('2d');

    if (window.miGraficoCanchas) {
        window.miGraficoCanchas.destroy();
    }

    window.miGraficoCanchas = new Chart(ctx, {
        type: 'doughnut',
        data: {
            labels: labels,
            datasets: [{
                label: 'Reservas por Cancha',
                data: datos,
                backgroundColor: [
                    'rgba(54, 162, 235, 0.7)',
                    'rgba(255, 206, 86, 0.7)',
                    'rgba(255, 99, 132, 0.7)',
                    'rgba(75, 192, 192, 0.7)',
                    'rgba(153, 102, 255, 0.7)',
                    'rgba(255, 159, 64, 0.7)'
                ],
                borderColor: [
                    'rgba(54, 162, 235, 1)',
                    'rgba(255, 206, 86, 1)',
                    'rgba(255, 99, 132, 1)',
                    'rgba(75, 192, 192, 1)',
                    'rgba(153, 102, 255, 1)',
                    'rgba(255, 159, 64, 1)'
                ],
                borderWidth: 1
            }]
        },
        options: {
            responsive: true,
            layout: {
                padding: {
                    top: 0,
                    bottom: 0,
                    left: 0,
                    right: 0
                }
            },
            plugins: {
                legend: {
                    position: 'right'
                }
            },
            cutout: '60%'  
        }



    });
};


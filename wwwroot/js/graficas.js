window.dibujarGraficoReservasPorCancha = (labels, activas, canceladas, expiradas) => {
    const ctx = document.getElementById('graficoReservasCancha').getContext('2d');

    // ✅ Verifica si el gráfico ya existe antes de destruirlo
    if (window.graficoReservasCancha && typeof window.graficoReservasCancha.destroy === 'function') {
        window.graficoReservasCancha.destroy();
    }

    window.graficoReservasCancha = new Chart(ctx, {
        type: 'bar',
        data: {
            labels: labels,
            datasets: [
                {
                    label: 'Activas',
                    data: activas,
                    backgroundColor: '#4CAF50'
                },
                {
                    label: 'Canceladas',
                    data: canceladas,
                    backgroundColor: '#F44336'
                },
                {
                    label: 'Expiradas',
                    data: expiradas,
                    backgroundColor: '#9E9E9E'
                }
            ]
        },
        options: {
            responsive: true,
            scales: {
                x: { stacked: true },
                y: {
                    stacked: true,
                    beginAtZero: true,
                    ticks: {
                        stepSize: 1,
                        callback: function (value) {
                            return Number.isInteger(value) ? value : null;
                        },
                        maxTicksLimit: 10
                    },
                    suggestedMax: 10
                }

            },
            plugins: {
                legend: { position: 'top' }
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
                    top: 10,
                    bottom: 10,
                    left: 10,
                    right: 10
                }
            },

            plugins: {
                legend: {
                    position: 'right'
                }
            },
            cutout: '70%'  
        }



    });
};


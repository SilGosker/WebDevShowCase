<script>
    import Chart from 'chart.js/auto';
    import {onMount} from "svelte";

    const data = [
        {name: 'C#, .NET (Core & Framework)', value: 100},
        {name: 'ASP.NET (Core & Framework)', value: 100},
        {name: 'Blazor', value: 90},
        {name: 'JavaScript', value: 90},
        {name: 'Entity Framework', value: 85},
        {name: 'Dapper', value: 85},
        {name: 'ECMA-335 / (C)IL', value: 60},
    ];


    onMount(() => {
        const chart = new Chart(document.getElementById("skill_canvas"), {
            type: 'bar',
            data: {
                labels: data.map(e => e.name),
                datasets: [
                    {
                        label: "Mijn niveau",
                        data: data.map(e => e.value)
                    }
                ]
            },
            options: {
                indexAxis: screen.width >= 640 ? 'x' : 'y',
                elements: {
                    bar: {
                        borderWidth: 1,
                    }
                },
                responsive: true,
                plugins: {
                    legend: {
                        display: false
                    },
                },
                maintainAspectRatio: false,
            },
        });
    });

</script>

<h2 class="text-center text-3xl w-full text-gray-900">Mijn skills</h2>
<div class="p-4 sm:w-full sm:h-1/12">
    <canvas height="300" id="skill_canvas"></canvas>
</div>

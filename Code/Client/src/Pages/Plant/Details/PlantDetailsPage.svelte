<script lang="ts">
    import { onMount } from "svelte";
    import { ApiContext } from "../../../Functions/ApiContext";
    import Loader from "../../../Components/Loader.svelte";
    import { UserContext } from "../../../Functions/UserContext";
    import Swal from "sweetalert2";
    import Chart from "chart.js/auto";

    const apiContext = new ApiContext();
    const userContext = new UserContext();

    let state = "Niet verbonden";
    let plant = null;
    let ws: WebSocket = null;
    let chart: Chart;
    const data: { date: Date; value: number }[] = [];

    function giveWater() {
        ws.send("p:" + plant.duration);
    }

    function updateChart() {
        if (chart) {
            chart.data.labels = data.map((d) =>
                d.date.toLocaleTimeString("nl-NL", {
                    hour: "2-digit",
                    minute: "2-digit",
                }),
            );
            chart.data.datasets[0].data = data.map((d) => d.value);
            chart.update();
        }
    }

    onMount(async () => {
        const id = parseInt(
            window.location.href.substring(
                window.location.href.lastIndexOf("/") + 1,
            ),
        );

        plant = (await apiContext.getPlant(id)) as any;
        data.push(
            ...plant.plantValues.map((x) => ({
                date: new Date(x.recordedAt),
                value: x.pumpState ? 1 : 0,
            })),
        );
        if (await apiContext.plantIsConnected(id)) {
            state = "Verbonden";
        }

        const token = userContext.token();
        ws = new WebSocket(
            `wss://localhost:7196/watch?plantId=${id}&auth=${token}`,
        );
        ws.onmessage = (event) => {
            const message = event.data as string;
            if (message.endsWith("0")) {
                data.push({ date: new Date(), value: 0 });
                state = "Verbonden";
            } else if (message.endsWith("1")) {
                state = "Geeft water";
                data.push({ date: new Date(), value: 1 });
            } else {
                state = "Niet verbonden";
            }
            updateChart();
        };

        ws.onerror = () => {
            Swal.fire({
                title: "Er is iets fout gegaan",
                text: `Een onverwachte fout is opgetreden. Probeer het later opnieuw. Als de fout blijft optreden, neem contact op met de ontwikkelaar.`,
            });
        };

        // Initialize chart
        const ctx = document.getElementById("pumpChart") as HTMLCanvasElement;
        chart = new Chart(ctx, {
            type: "line",
            data: {
                labels: data.map((d) =>
                    d.date.toLocaleTimeString("nl-NL", {
                        hour: "2-digit",
                        minute: "2-digit",
                    }),
                ),
                datasets: [
                    {
                        label: "Pompstatus",
                        data: data.map((d) => d.value),
                        fill: {
                            target: "origin",
                            above: "rgba(75, 192, 192, 0.4)",
                            below: "transparent",
                        },
                        borderColor: "rgba(75, 192, 192, 1)",
                        backgroundColor: "rgba(75, 192, 192, 0.4)",
                        pointBackgroundColor: "rgba(75, 192, 192, 1)",
                        tension: 0.3,
                    },
                ],
            },
            options: {
                scales: {
                    x: {
                        type: "category",
                        title: {
                            display: true,
                            text: "Tijd",
                        },
                    },
                    y: {
                        suggestedMin: 0,
                        suggestedMax: 1,
                        ticks: {
                            stepSize: 1,
                        },
                    },
                },
                plugins: {
                    tooltip: {
                        callbacks: {
                            title: (tooltipItems) => {
                                const index = tooltipItems[0].dataIndex;
                                return data[index].date.toLocaleString();
                            },
                        },
                    },
                },
            },
        });
    });
</script>

<div class="flex justify-center flex-col items-center p-5 w-full">
    {#if plant}
        <div class="w-1/2">
            <div class="flex flex-col justify-center items-center">
                <div class="flex w-full justify-center items-center">
                    <h1 class="text-center text-2xl text-gray-900">
                        {plant.name}
                    </h1>
                    <a
                        class="ml-2 inline-flex items-center px-3 py-2 text-sm font-medium text-center text-white bg-sky-400 rounded-lg hover:bg-sky-600 focus:ring-4 focus:outline-none focus:ring-blue-300"
                        href="/#/plants/update/{plant.id}"
                    >
                        Update informatie
                    </a>
                </div>

                <div class="my-3 flex items-center justify-between">
                    <span class="mr-2">Status:</span>
                    <span
                        class="ml-2 {state === 'Geeft water'
                            ? 'text-sky-500'
                            : state === 'Verbonden'
                              ? 'text-green-500'
                              : 'text-red-500'}"
                    >
                        {state}
                    </span>

                    <button
                        class="ml-2 inline-flex items-center px-3 py-2 text-sm font-medium text-center text-white bg-sky-400 rounded-lg hover:bg-sky-600 focus:ring-4 focus:outline-none focus:ring-blue-300"
                        on:click={giveWater}>Dosis water geven</button
                    >
                </div>
            </div>
        </div>

        <canvas class="w-full" style="max-height: 750px;" id="pumpChart"></canvas>
    {:else}
        <Loader width={200} />
    {/if}
</div>

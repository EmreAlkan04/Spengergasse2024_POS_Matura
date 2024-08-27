<script setup>
import axios from 'axios';
import { RouterLink } from 'vue-router';
</script>

<template>
    <div class="teamsView">
        <h1>Teams</h1>
        <div class="searchbar">
            <input type="text" v-model="teamfilter" placeholder="Search for team name..." />
            <button type="button" v-on:click="filterTeams()">Search</button>
        </div>

        <div class="teamsContainer">
            <!-- Auflistung aller Teams -->
            <RouterLink class="team" v-for="t in filteredTeams" v-bind:key="t.guid"
                v-bind:to="`/teams/${t.guid}`">
                <div class="teamName">{{ t.name }}</div>
                <div class="teamSchoolclass">{{ t.schoolclass }}</div>
            </RouterLink>
        </div>
    </div>
</template>

<script>
export default {
    data: function () {
        return {
            teams: [],
            teamfilter: ""
        }
    },
    async mounted() {
        // Durch axios.baseUrl wird der Pfad /api und bei Bedarf https://localhost:5000 
        // automatisch vorangestellt
        const response = await axios.get('teams');
        this.teams = response.data;
    },
    methods: {

    },
    computed: {
        filteredTeams() {
            // check if the filter is empty
            if (!this.teamfilter) return this.teams;
            return this.teams.filter(t => t.name.toLowerCase()
                .includes(this.teamfilter.trim().toLowerCase()));
        }
    }

}
</script>

<style scoped>
h1 {
    margin: 0;
}

.searchbar {
    display: flex;
    gap: 1em;
}

.teamsContainer {
    display: flex;
    flex-wrap: wrap;
    gap: 1em;
}

.team {
    display: block;
    border: 1px solid black;
    border-radius: 5px;
    width: 10em;
    padding: 0.5em;
}

.team:hover {
    background-color: #f0f0f0;
}

.teamName {
    font-weight: bold;
}
</style>

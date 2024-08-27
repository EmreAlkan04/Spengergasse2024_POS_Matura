<script setup>
import axios from 'axios';
</script>

<template>
    <div class="pupilsView">
        <h1>Pupils</h1>
        <table>
            <thead>
                <tr>
                    <th>GUID</th>
                    <th>Firstname</th>
                    <th>Lastname</th>
                    <th>Email</th>
                </tr>
            </thead>
            <tbody>
                <tr v-for="pupil in pupils" :key="pupil.guid">
                    <td>{{ pupil.guid }}</td>
                    <td>{{ pupil.firstname }}</td>
                    <td>{{ pupil.lastname }}</td>
                    <td>{{ pupil.email }}</td>
                </tr>
            </tbody>
        </table>
    </div>
</template>

<style scoped>
    tr:hover {
        background-color: lightyellow;
    }
</style>

<script>
export default {
    data: function () {
        return {
            pupils: []
        }
    },
    async mounted() {
        // Durch axios.baseUrl wird der Pfad /api und bei Bedarf https://localhost:5000 
        // automatisch vorangestellt
        const response = await axios.get('students');
        this.pupils = response.data;
    },
    methods: {

    }
}
</script>
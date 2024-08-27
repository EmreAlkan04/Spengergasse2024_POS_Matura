<script setup>
import axios from 'axios';
</script>

<template>
    Ich bin die Detail View für das Team {{ teamDetails.name }}.

    <table v-if="teamDetails.tasks && teamDetails.tasks.length">
        <thead>
            <tr>
                <th>Subject</th>
                <th>Title</th>
                <th>Teacher</th>
                <th>Expiration Date</th>
            </tr>
        </thead>
        <tbody>
            <tr v-for="task in teamDetails.tasks" v-bind:key="task.guid">
                <td>{{ task.subject }}</td>
                <td>{{ task.title }}</td>
                <td>{{ task.teacherInitials }} - {{ task.teacherLastname }}
                    {{ task.teacherFirstname }}</td>
                <td>{{ task.expirationDate }}</td>
            </tr>
        </tbody>
    </table>
</template>

<script>
export default {
    data: function () {
        return {
            teamDetails: {}
        }
    },
    async mounted() {
        // try {
        const response = await axios.get(`teams/${this.$route.params.teamGuid}`);
        this.teamDetails = response.data;
        // }
        // catch (error) {
        //     alert("Fehler beim Laden der Daten für das Team.");
        // }
    },
    methods: {

    },
    computed: {

    }

}
</script>
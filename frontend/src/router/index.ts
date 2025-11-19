import { createRouter, createWebHistory, RouteRecordRaw } from "vue-router";
import CreateClientPage from "@/pages/CreateClientPage.vue";
import SubmissionPage from "@/pages/SubmissionPage.vue";

const routes: RouteRecordRaw[] = [
    { path: '/', redirect: '/clients/create' },
    { path: '/clients/create', component: CreateClientPage},
    { path: '/submissions', component: SubmissionPage}
];

export const router = createRouter({
    history: createWebHistory(),
    routes
})
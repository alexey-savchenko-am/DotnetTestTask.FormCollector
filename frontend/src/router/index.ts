import { createRouter, createWebHistory, RouteRecordRaw } from "vue-router";
import CreateClientPage from "@/pages/CreateClientPage.vue";

const routes: RouteRecordRaw[] = [
    { path: '/', redirect: '/clients/create' },
    { path: '/clients/create', component: CreateClientPage}
];

export const router = createRouter({
    history: createWebHistory(),
    routes
})
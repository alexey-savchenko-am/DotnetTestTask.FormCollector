<template>
  <div class="py-4 px-2 space-x-4 flex justify-center md:justify-start text-gray-500 md:text-lg text-sm">
    <router-link
      to="/clients/create"
      class="px-3 py-1 uppercase text-gray-500 hover:text-blue-500"
      active-class="font-bold"
    >
      Clients
    </router-link>

    <router-link
      to="/submissions"
      class="px-3 py-1 uppercase text-gray-500 hover:text-blue-500"
      active-class="font-bold"
    >
      Submissions
    </router-link>
  </div>

  <div class="p-6 space-y-4">
    <router-view />
  </div>

</template>

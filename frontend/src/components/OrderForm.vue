<script setup lang="ts">
import {reactive, ref} from 'vue';

interface OrderForm{
    name: string
    email: string,
    product: string
    quantity: number | null
} 
</script>
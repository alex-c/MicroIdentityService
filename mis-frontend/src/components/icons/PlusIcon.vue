<template>
  <Icon :size="size" :path="path" @click="$emit('click')" />
</template>

<script>
import Icon from '@/components/Icon.vue';
import { mdiMenu } from '@mdi/js';
import { mdiPlusCircle } from '@mdi/js';

export default {
  name: 'LogoutIcon',
  components: { Icon },
  props: {
    size: {
      type: Number,
      default: 24,
    },
    menuOpen: Boolean,
  },
  data() {
    return {
      path: mdiPlusCircle,
    };
  },
};
</script>

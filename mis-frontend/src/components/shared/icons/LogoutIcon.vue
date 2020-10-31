<template>
  <Icon :size="size" :path="path" />
</template>

<script>
import Icon from '@/components/shared/Icon.vue';
import { mdiLogout } from '@mdi/js';

export default {
  name: 'LogoutIcon',
  components: { Icon },
  props: {
    size: {
      type: Number,
      default: 24,
    },
  },
  data() {
    return {
      path: mdiLogout,
    };
  },
};
</script>

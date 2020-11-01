<template>
  <Icon :size="size" :path="path" @click="$emit('click')" />
</template>

<script>
import Icon from '@/components/shared/Icon.vue';
import { mdiCog } from '@mdi/js';

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
      path: mdiCog,
    };
  },
};
</script>

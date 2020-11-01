<template>
  <Icon :size="size" :path="path" @click="$emit('click')" />
</template>

<script>
import Icon from '@/components/Icon.vue';
import { mdiMenu } from '@mdi/js';
import { mdiBackburger } from '@mdi/js';

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
  computed: {
    path() {
      return this.menuOpen ? mdiBackburger : mdiMenu;
    },
  },
};
</script>

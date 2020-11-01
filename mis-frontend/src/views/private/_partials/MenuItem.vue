<template>
  <div :class="[{ 'menu-item-active': active }, 'menu-item']" @click="navigate">
    <span>{{ label }}</span>
  </div>
</template>

<script>
import { SET_MENU_DRAWER_OPEN } from '@/store/mutations.js';

export default {
  name: 'MenuItem',
  props: ['target', 'current'],
  data() {
    return {
      label: this.$t(`general.${this.target}`),
      route: `/${this.target}`,
    };
  },
  computed: {
    collapsedUi() {
      return this.$store.state.collapsedUi;
    },
    active() {
      return this.route === this.current;
    },
  },
  methods: {
    navigate: function() {
      if (this.route !== this.current) {
        this.$router.push({ path: this.route });
        if (this.collapsedUi) {
          this.$store.commit(SET_MENU_DRAWER_OPEN, false);
        }
      }
    },
  },
};
</script>

<style lang="scss" scoped>
@import '@/style/colors.scss';

.menu-item {
  padding: 16px;
  font-size: 16px;
  &:hover {
    cursor: pointer;
    color: $mis-color-primary;
    text-decoration: underline;
    background-color: #f0f0f0;
  }
}

.menu-item-active {
  color: $mis-color-primary;
}
</style>

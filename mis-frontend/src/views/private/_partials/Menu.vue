<template>
  <div id="menu" :class="{ collapsed: collapsedUi, shown: menuOpen }">
    <MenuItem target="dashboard" :current="current" />
    <MenuItem target="identities" :current="current" />
    <MenuItem target="domains" :current="current" />
    <MenuItem target="roles" :current="current" />
  </div>
</template>

<script>
import MenuItem from '@/views/private/_partials/MenuItem.vue';

export default {
  name: 'Menu',
  components: { MenuItem },
  computed: {
    collapsedUi() {
      return this.$store.state.ui.collapsedUi;
    },
    menuOpen() {
      return this.$store.state.ui.menuOpen;
    },
    current() {
      return this.$route.path;
    },
  },
};
</script>

<style lang="scss" scoped>
@import '@/style/colors.scss';

#menu {
  width: 0px;
  overflow: hidden;
  text-align: left;
  background-color: white;
  border-right: 1px solid $mis-color-border;
  box-shadow: 1px 0px 2px 0px $mis-color-shadow;
  transition: width 0.5s;
  flex-shrink: 0;
  z-index: 3;
  &.collapsed {
    position: absolute;
    top: 65px;
    left: 0px;
    bottom: 0px;
  }
  &.shown {
    width: 200px;
  }
}
</style>

<template>
  <div id="menu" :class="{ collapsed: collapsedUi, shown: menuOpen }">
    <div class="menu-item">Dashboard</div>
    <div class="menu-item">Identities</div>
    <div class="menu-item">Domains</div>
    <div class="menu-item">Roles</div>
  </div>
</template>

<script>
export default {
  name: 'Menu',
  computed: {
    collapsedUi: function() {
      return this.$store.state.collapsedUi;
    },
    menuOpen: function() {
      return this.$store.state.menuOpen;
    },
  },
};
</script>

<style lang="scss" scoped>
@import '@/style/colors.scss';

#menu {
  position: absolute;
  top: 65px;
  left: 0px;
  bottom: 0px;
  width: 0px;
  overflow: hidden;
  text-align: left;
  background-color: white;
  border-right: 1px solid $mis-color-border;
  box-shadow: 1px 0px 2px 0px $mis-color-shadow;
  transition: width 0.5s;
  &.collapsed {
    top: 0px;
  }
  &.shown {
    width: 200px;
  }
}

.menu-item {
  padding: 16px;
}
</style>

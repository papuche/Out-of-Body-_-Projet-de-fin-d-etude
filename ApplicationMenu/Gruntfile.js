module.exports = function(grunt) {

  // Configuration de Grunt
  grunt.initConfig({
    concat: {
      options: {
        separator: ';', // permet d'ajouter un point-virgule entre chaque fichier concaténé.
      },
      compile: {
        src: ['client/controllers/controllerMenuWeb.js','client/controllers/controllerMainMenu.js','client/controllers/*.js'], // la source
        dest: 'client/controllers/built.js' // la destination finale
      }
    },
    uglify: {
      options: {
        separator: ';',
        mangle: false, // si false : enleve les changements de nom de variable
        // sourceMap: true,
        // sourceMapName: 'client/sourcemap.map'
      },
      dist: {
        src: ['client/controllers/controllerMenuWeb.js','client/controllers/controllerMainMenu.js','client/controllers/*.js'],
        dest: 'client/controllers/built.js'
      }
    },
    watch: {
      scripts: {
        files: 'client/controllers/*.js', // tous les fichiers JavaScript du dossier src
        tasks: ['uglify:dist']
      }
    }
})

  // Définition des tâches Grunt
  grunt.loadNpmTasks('grunt-contrib-concat')
  grunt.loadNpmTasks('grunt-contrib-uglify')
  grunt.loadNpmTasks('grunt-contrib-watch')

  grunt.registerTask('default', ['dist','watch'])
  grunt.registerTask('dev', ['concat:compile'])
  grunt.registerTask('dist', ['uglify:dist'])
}
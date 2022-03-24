var terser = require('terser-webpack-plugin')
const path = require('path')

module.exports = {
    mode: 'development',
    entry: {
        entry: './src/index.ts'
    },
    output: {
        path: path.resolve(__dirname, '../wwwroot/'),
        filename: 'chemsharp.bundle.js',
        library: 'chemsharpMolecules'
    },
    resolve: {
        alias: {
            components: path.resolve(__dirname, 'src/'),
        },
        extensions: ['.ts', '.js'],
    },
    module: {
        rules: [
            {test: /\.js$/, use: 'babel-loader', exclude: /node_modules/},
            {test: /\.ts$/, use: 'ts-loader', exclude: /node_modules/}
        ]
    },
    optimization: {
        usedExports: false
    },
    plugins: [
        new terser()
    ]
}

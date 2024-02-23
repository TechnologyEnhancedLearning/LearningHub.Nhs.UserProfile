
module.exports = {
    module: {
        rules: [
            {
                test: /\.scss$/,
                use: [
                    {
                        loader: 'style-loader'
                    },
                    {
                        loader: 'css-loader',
                        options: {
                            url: false,
                        },
                    },
                    {
                        loader: 'sass-loader',
                        options: {
                            implementation: require("sass"),
                        },
                    },
                ],
            },
            {
                test: /\.css$/,
                use: [
                    {
                        loader: 'style-loader'
                    },
                    {
                        loader: 'css-loader'
                    },
                ],
            },
        ],
    },
};

//var path = require('path');

//module.exports = {
//    context: path.join(__dirname, 'App'),
//    entry: './main.js',
//    output: {
//        path: path.join(__dirname, 'Built'),
//        filename: '[name].bundle.js'
//    }
//};
# cleaning script
# admin privileges required!

helm uninstall client
helm uninstall ids
helm uninstall mobile
helm uninstall api

# remove from hosts
sed -ie "\|^127.0.0.1 client.demo.ebt.com\$|d" /etc/hosts
sed -ie "\|^127.0.0.1 ids.demo.ebt.com\$|d" /etc/hosts
sed -ie "\|^127.0.0.1 api.demo.ebt.com\$|d" /etc/hosts
sed -ie "\|^127.0.0.1 mobile.demo.ebt.com\$|d" /etc/hosts
